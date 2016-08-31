

CREATE PROCEDURE [dbo].[usp_ProcessMissingMajors]
	
AS
	
	-- declare variables
	declare @term varchar(6), @id uniqueidentifier, @pidm varchar(7), @tsql varchar(max)

	-- create the temp table
	IF object_id('tempdb..#StudentMajors') IS NOT NULL
	BEGIN
		DROP TABLE #StudentMajors
	END

	CREATE TABLE #StudentMajors
	(
		id uniqueidentifier,
		major varchar(4)
	)

	-- get the current term
	set @term = (select MAX(id) from termcodes where isactive = 1)
																	
	declare @students cursor
	set @students = cursor for
		select id, pidm from students
		where id not in (select student_id from studentmajors)
		  and termcode = @term
	 
	open @students

	fetch next from @students into @id, @pidm	

	while(@@fetch_status = 0)
	begin
		delete from #StudentMajors

		insert into #StudentMajors (major)
		select zgvlcfs_majr_code major
		from DatamartStudentMajors where zgvlcfs_pidm = @pidm

		update #StudentMajors set id = @id
	
		merge studentmajors as t
		using #studentmajors as s
		on t.student_id = s.id and t.majorcode = s.major
		when not matched
		then insert (student_id, majorcode) values(s.id, s.major);	

		fetch next from @students into @id, @pidm	

	end

	close @students
	deallocate @students

RETURN 0
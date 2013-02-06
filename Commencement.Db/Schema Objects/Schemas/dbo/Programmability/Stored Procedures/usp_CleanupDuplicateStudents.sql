CREATE PROCEDURE [dbo].[usp_CleanupDuplicateStudents]
	
AS

declare @term varchar(6) = (select MAX(id) from termcodes where isactive = 1)

declare @cursor cursor, @sid uniqueidentifier

set @cursor = cursor for
	select min(cast(id as varchar(36)))
	from students 
	where studentid in (
		select studentid
		from students
		where termcode = @term
		group by studentid
		having count(id) > 1
	)
	  and termcode = @term
	group by studentid

open @cursor

fetch next from @cursor into @sid

while (@@FETCH_STATUS = 0)
begin
	
	-- check to make sure we don't have a registration under this id
	if not exists (select * from registrations where student_id = @sid)
	begin

		delete from StudentMajors where student_id = @sid
		delete from students where id = @sid

	end

	fetch next from @cursor into @sid

end

close @cursor
deallocate @cursor





RETURN 0

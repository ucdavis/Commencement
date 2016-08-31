


CREATE PROCEDURE [dbo].[usp_ProcessStudentsMultiCollege]
AS    

declare @term varchar(6), @minUnits int, @coll char(2)
declare @tsql varchar(max)

if (not exists (select * from termcodes where isactive = 1) )
begin
    return 1
end

set @term = (select MAX(id) from termcodes where isactive = 1)
set @minUnits = (select min(PetitionThreshold) from Ceremonies where termcode = @term)    

if (GETDATE() + 8 < (select MIN(RegistrationBegin) from TermCodes) or
	getdate() > (select MAX(registrationpetitiondeadline) from TermCodes) )
	begin
		return 2
	end

insert into Majors ( id, name, IsActive )
select distinct zgvlcfs_majr_code, 'unknown', 0 from DatamartStudents
where zgvlcfs_majr_code not in ( select id from Majors )

merge into students t
using (	select distinct spriden_pidm pidm, spriden_id studentid, spriden_first_name firstname
					, spriden_mi mi, spriden_last_name lastname
					, earnedunits, currentUnits, goremal_email_address email, loginid
					, @term as termcode, sja
        from DatamartStudents where (shrttrm_astd_code_end_of_term is null or shrttrm_astd_code_end_of_term <> 'DS')) s
on t.pidm = s.pidm and t.termcode = s.termcode
when matched then update
	-- only update the units
	set t.earnedunits = s.earnedunits, t.currentunits = s.currentunits, dateupdated = getdate(), sjablock = s.sja
when not matched then
    insert (pidm, studentid, firstname, mi, lastname, earnedunits, CurrentUnits, email, termcode, [login], sjablock)
    values(s.pidm, s.studentid, s.firstname, s.mi, s.lastname, s.earnedunits, s.currentunits, s.email, s.termcode, s.[loginId], s.sja);

-- delete the student majors for which wee have an update for them
delete from StudentMajors
where Student_Id in ( select id from Students where pidm in ( select spriden_pidm pidm from DatamartStudents ) )

-- insert the updated student majors
insert into StudentMajors 
select distinct students.id, zgvlcfs_majr_code major from DatamartStudents dms
        inner join students on dms.spriden_pidm = students.pidm and students.termcode = @term

-- clean up any duplicates that may have popped up
exec usp_CleanupDuplicateStudents

RETURN 0
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_DownloadStudents')
	BEGIN
		DROP  Procedure  usp_DownloadStudents
	END

GO

CREATE PROCEDURE [dbo].[usp_DownloadStudents]
AS
    
IF object_id('tempdb..#Students') IS NOT NULL
BEGIN
    DROP TABLE #Students
END

CREATE TABLE #Students
(
    pidm varchar(8),
    studentid varchar(9),
    firstName varchar(50),
    MI varchar(50),
    LastName varchar(50),
    EarnedUnits decimal(6,3),
    CurrentUnits decimal(6,3),
    Email varchar(100),
    LoginId varchar(50),
    std varchar(2),
    major varchar(4)
)

declare @term varchar(6), @sisterm varchar(6), @minUnits int, @coll char(2)
declare @tsql varchar(max)

if (not exists (select * from termcodes where isactive = 1) )
begin
    return 1
end

set @term = (select MAX(id) from termcodes where isactive = 1)
select @sisterm = term from openquery(sis, '
											select min(stvterm_code) term
											from stvterm
											where stvterm_end_date > sysdate
											  and stvterm_trmt_code = ''Q''
										')
set @minUnits = (select min(PetitionThreshold) from Ceremonies where termcode = @term)    

if (GETDATE() + 8 < (select MIN(RegistrationBegin) from Ceremonies) or
	getdate() > (select MAX(registrationdeadline) from ceremonies) )
	begin
		return 2
	end
	
set @tsql = '
	insert into #students (pidm, studentid, firstname, mi, lastname, earnedunits, currentunits, email, loginid, std, major)
	select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name
		, earnedunits, currentunits, goremal_email_address, loginid, shrttrm_astd_code_end_of_term
		, zgvlcfs_majr_code
	from openquery(sis, ''
		select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name
			, EarnedUnits.shrlgpa_hours_earned as EarnedUnits
			, 0 CurrentUnits
			, email.goremal_email_address
			, lower(wormoth_login_id) loginId
			, shrttrm_astd_code_end_of_term
			, zgvlcfs_majr_code
		from zgvlcfs
			inner join spriden on spriden_pidm = zgvlcfs_pidm
			inner join shrlgpa earnedUnits on earnedUnits.shrlgpa_pidm = zgvlcfs_pidm
			left outer join (
				select goremal_pidm, goremal_email_address
				from goremal
				where goremal_emal_code = ''''UCD''''
					and goremal_status_ind = ''''A''''
			) email on email.goremal_pidm = zgvlcfs_pidm
			inner join wormoth on wormoth_pidm = zgvlcfs_pidm
			inner join shrttrm on shrttrm_pidm = zgvlcfs_pidm
		where spriden_change_ind is null
			and zgvlcfs_term_code_eff = '''''+@sisterm+'''''
			and EarnedUnits.shrlgpa_hours_earned > ' + CAST(@minUnits as varchar(6)) + '
			and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where shrttrm.shrttrm_pidm = ishrttrm.shrttrm_pidm )
			and wormoth_acct_type = ''''Z''''
			and wormoth_acct_status = ''''A''''
			and earnedUnits.shrlgpa_gpa_type_ind = ''''O''''
			and earnedUnits.shrlgpa_levL_code = ''''UG''''
	'')
'

exec (@tsql)

merge into students t
using (	select distinct pidm, studentid, firstname, mi
                    , lastname, earnedunits, currentUnits, email, loginid, @term as termcode
        from #students where std <> 'DS') s
on t.pidm = s.pidm and t.termcode = s.termcode
when not matched then
    insert (pidm, studentid, firstname, mi, lastname, earnedunits, CurrentUnits, email, termcode, [login])
    values(s.pidm, s.studentid, s.firstname, s.mi, s.lastname, s.earnedunits, s.currentunits, s.email, s.termcode, s.[loginId]);
    
delete from StudentMajors
where Student_Id in ( select id from Students where TermCode = @term )

insert into StudentMajors 
select distinct students.id, major from #students
        inner join students on #students.pidm = students.pidm and students.termcode = @term
    
DROP TABLE #Students

RETURN 0
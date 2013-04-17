
CREATE PROCEDURE [dbo].[usp_DownloadStudentsMultiCollege]
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
	sja bit,
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

if (GETDATE() + 8 < (select MIN(RegistrationBegin) from TermCodes) or
	getdate() > (select MAX(registrationpetitiondeadline) from TermCodes) )
	begin
		return 2
	end
	
set @tsql = '
	insert into #students (pidm, studentid, firstname, mi, lastname, earnedunits, currentunits, email, loginid, std, sja, major)
	select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name
		, earnedunits, currentunits, goremal_email_address, loginid, shrttrm_astd_code_end_of_term
		, sja
		, zgvlcfs_majr_code
	from openquery(sis, ''
		select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name
			, EarnedUnits.shrlgpa_hours_earned as EarnedUnits
			, 0 as CurrentUnits
			, email.goremal_email_address
			, lower(wormoth_login_id) loginId
			, shrttrm_astd_code_end_of_term
			, (case when sjaholds.sprhold_pidm is not null then 1 else 0 end) sja
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
			inner join (
				select a.wormoth_pidm, a.wormoth_login_id
				from wormoth a
					join (select t.wormoth_pidm, max(t.wormoth_activity_date) as maxsubkey from wormoth t group by t.wormoth_pidm ) b on b.maxsubkey = a.wormoth_activity_date and a.wormoth_pidm = b.wormoth_pidm
				where wormoth_acct_type = ''''Z''''
				  and wormoth_acct_status = ''''A''''
			)wormoth on wormoth_pidm = zgvlcfs_pidm
			left outer join shrttrm on shrttrm_pidm = zgvlcfs_pidm
			left outer join (
				select distinct sprhold_pidm
				from sprhold
				where sprhold_hldd_code in (''''BA'''', ''''BB'''', ''''RG'''')
				  and sprhold_from_date < sysdate
				  and sprhold_to_date > sysdate
			) sjaholds on sjaholds.sprhold_pidm = zgvlcfs_pidm
		where spriden_change_ind is null
			and zgvlcfs_term_code_eff = '''''+@sisterm+'''''
			and EarnedUnits.shrlgpa_hours_earned > ' + CAST(@minUnits as varchar(6)) + '
			and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where shrttrm.shrttrm_pidm = ishrttrm.shrttrm_pidm )
			and earnedUnits.shrlgpa_gpa_type_ind = ''''O''''
			and earnedUnits.shrlgpa_levL_code = ''''UG''''
	'')
'

exec (@tsql)

insert into Majors ( id, name, IsActive )
select distinct major, 'unknown', 0 from #Students
where major not in ( select id from Majors )

merge into students t
using (	select distinct pidm, studentid, firstname, mi
                    , lastname, earnedunits, currentUnits, email, loginid, @term as termcode, sja
        from #students where (std is null or std <> 'DS')) s
on t.pidm = s.pidm and t.termcode = s.termcode
when matched then update
	-- only update the units
	set t.earnedunits = s.earnedunits, t.currentunits = s.currentunits, dateupdated = getdate(), sjablock = s.sja
when not matched then
    insert (pidm, studentid, firstname, mi, lastname, earnedunits, CurrentUnits, email, termcode, [login], sjablock)
    values(s.pidm, s.studentid, s.firstname, s.mi, s.lastname, s.earnedunits, s.currentunits, s.email, s.termcode, s.[loginId], s.sja);

-- delete the student majors for which wee have an update for them
delete from StudentMajors
where Student_Id in ( select id from Students where studentId in ( select studentid from #students ) )

-- insert the updated student majors
insert into StudentMajors 
select distinct students.id, major from #students
        inner join students on #students.pidm = students.pidm and students.termcode = @term
    
DROP TABLE #Students

-- clean up any duplicates that may have popped up
exec usp_CleanupDuplicateStudents

RETURN 0
﻿CREATE PROCEDURE [dbo].[usp_DownloadStudentsMultiCollege]
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

declare @term varchar(6), @minUnits int, @coll char(2)
declare @tsql varchar(max)

if (exists (select * from termcodes where isactive = 1) )
begin
    set @term = (select MAX(id) from termcodes where isactive = 1)
end


declare @collCursor cursor 
set @collCursor = cursor for
                    select MIN(PetitionThreshold) unitLowerBound, CollegeCode
                    from Ceremonies
                        inner join CeremonyColleges on Ceremonies.id = CeremonyColleges.CeremonyId
                    where TermCode = @term
                    group by CollegeCode

open @collCursor

fetch next from @collCursor into @minUnits, @coll

while (@@FETCH_STATUS = 0)
    begin
        -- load students
        set @tsql = '
            insert into #Students
            select distinct * from openquery(sis, ''
            select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name
                , EarnedUnits.units as EarnedUnits
                , CurrentUnits.units as CurrentUnits
                , email.goremal_email_address
                , lower(wormoth_login_id) loginId
                , shrttrm_astd_code_end_of_term
                , zgvlcfs_majr_code
            from zgvlcfs
                inner join spriden on spriden_pidm = zgvlcfs_pidm
                inner join ( 
                    select shrlgpa_pidm as pidm, shrlgpa_hours_earned units
                    from shrlgpa
                    where shrlgpa_gpa_type_ind = ''''O''''
                        and shrlgpa_levl_code = ''''UG''''
                        ) EarnedUnits on EarnedUnits.pidm = zgvlcfs_pidm
                left outer join (
                    select sfrstcr_pidm as pidm, sum(sfrstcr_credit_hr) units
                    from sfrstcr
                        left join shrtckn on sfrstcr_pidm = shrtckn_pidm 
                                         and sfrstcr_term_code = shrtckn_term_code
                                         and sfrstcr_crn = shrtckn_crn
                    where shrtckn_pidm is null
                    group by sfrstcr_pidm    
                ) CurrentUnits on CurrentUnits.pidm = zgvlcfs_pidm
                left outer join (
                    select goremal_pidm, goremal_email_address
                    from goremal
                    where goremal_emal_code = ''''UCD''''
                        and goremal_status_ind = ''''A''''
                ) email on email.goremal_pidm = zgvlcfs_pidm
                inner join wormoth on wormoth_pidm = zgvlcfs_pidm
                left outer join shrttrm on shrttrm_pidm = zgvlcfs_pidm
            where spriden_change_ind is null
                and zgvlcfs_term_code_eff = '''''+@term+''''' and zgvlcfs_coll_code = '''''+@coll+'''''
                and (EarnedUnits.units + nvl(CurrentUnits.units,0)) > ' + cast(@minUnits as varchar(6)) + '
                and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where shrttrm.shrttrm_pidm = ishrttrm.shrttrm_pidm )
				and wormoth_acct_type = ''''Z''''
				and wormoth_acct_status = ''''A''''
            '')'

        exec(@tsql)
        
        fetch next from @collCursor into @minUnits, @coll
    end
    
close @collCursor
deallocate @collCursor
    
    merge into students t
    using (	select distinct pidm, studentid, firstname, mi
                        , lastname, earnedunits, currentUnits, email, loginid, @term as termcode
            from #students where std <> 'DS') s
    on t.pidm = s.pidm and t.termcode = s.termcode
    when matched then
        update set t.studentid = s.studentid, t.firstname = s.firstname, t.mi = s.mi
                 , t.lastname = s.lastname, t.EarnedUnits = s.earnedUnits, t.CurrentUnits = s.currentUnits
                 , t.email = s.email, t.dateupdated = getdate(), t.[login] = s.loginid
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
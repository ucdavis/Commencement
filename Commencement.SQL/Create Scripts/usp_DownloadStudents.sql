IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_DownloadStudents')
	BEGIN
		DROP  Procedure  usp_DownloadStudents
	END

GO

CREATE Procedure usp_DownloadStudents

AS

declare @temp table(
	pidm varchar(8),
	studentid varchar(9),
	firstname varchar(50), mi varchar(50), lastname varchar(50),
	units decimal(6,3),
	email varchar(50), major varchar(4),
	coll char(2), degsCode varchar(4),
	astd char(2),
	termcode varchar(6),
	[login] varchar(50)
)

insert into @temp (pidm, studentid, firstname, mi, lastname, units, email, major, coll, degscode, astd, termcode, [login])
select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name, shrlgpa_hours_earned, goremal_email_address, zgvlcfs_majr_code
	, zgvlcfs_coll_code, shrdgmr_degs_code
	, shrttrm_astd_code_end_of_term
	, termcode
	, wormoth_login_id
from openquery (sis, '
	select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name, shrlgpa_hours_earned, email.goremal_email_address, curriculum.zgvlcfs_majr_code
		, curriculum.zgvlcfs_coll_code
		, shrdgmr_degs_code
		, shrttrm_astd_code_end_of_term
		, (select min(stvterm_code) from stvterm where stvterm_end_date > sysdate and stvterm_trmt_code = ''Q'') termcode
		, lower(wormoth_login_id) wormoth_login_id
	from spriden
		inner join shrlgpa on spriden_pidm = shrlgpa_pidm
		left outer join (
			select goremal_pidm, goremal_email_address
			from goremal
			where goremal_emal_code = ''UCD''
				and goremal_status_ind = ''A''
		) email on email.goremal_pidm = spriden_pidm
		inner join (
			select zgvlcfs_pidm, zgvlcfs_majr_code, zgvlcfs_coll_code
			from zgvlcfs
			where zgvlcfs_term_code_eff = (select min(stvterm_code) from stvterm where stvterm_end_date > sysdate and stvterm_trmt_code = ''Q'')
				and zgvlcfs_majr_code not in (''ABMB'', ''ABIS'', ''ACBI'', ''AEEB'', ''AGEN'', ''AMIC'', ''ANPB'', ''APLB'', ''AEVE'', ''BEEB'')
				and zgvlcfs_levl_code in (''UG'', ''U2'')
				and zgvlcfs_coll_code = ''AE''
		) curriculum on curriculum.zgvlcfs_pidm = spriden_pidm
		left outer join shrdgmr on shrdgmr_pidm = spriden_pidm 
		left outer join shrttrm on shrttrm_pidm = spriden_pidm
		left outer join wormoth on wormoth_pidm = spriden_pidm
	where spriden_change_ind is null
		and shrlgpa_gpa_type_ind = ''O''
		and shrlgpa_levl_code in (''UG'', ''U2'')
		and shrlgpa_hours_earned in ( select max(shrlgpa_hours_earned) from shrlgpa ishrlgpa where shrlgpa.shrlgpa_pidm = ishrlgpa.shrlgpa_pidm )
		and shrlgpa_hours_earned >= 140		
		and shrdgmr_degs_code <> ''DA''
		and wormoth_acct_type = ''Z''
		and wormoth_acct_status = ''A''
		and shrttrm_term_code = (select max(stvterm_code) from stvterm where stvterm_end_date < sysdate and stvterm_trmt_code = ''Q'')	
')

merge into students t
using (select distinct pidm, studentid, firstname, mi, lastname, units, email, degscode, termcode, [login] from @temp where astd <> 'DS') s
on t.pidm = s.pidm and t.termcode = s.termcode
when matched then
	update set t.studentid = s.studentid, t.firstname = s.firstname, t.mi = s.mi, t.lastname = s.lastname, t.units = s.units, t.email = s.email, t.degreecode = s.degscode, t.dateupdated = getdate(), t.[login] = s.[login]
when not matched then
	insert (pidm, studentid, firstname, mi, lastname, units, email, degreecode, termcode, [login])
	values(s.pidm, s.studentid, s.firstname, s.mi, s.lastname, s.units, s.email, s.degscode, s.termcode, s.[login]);

merge into studentmajors t
using (
	select distinct students.id, major from @temp tmp
		inner join students on tmp.pidm = students.pidm and tmp.termcode = students.termcode
	where major like 'A%'
) s
on t.student_id = s.id and t.majorcode = s.major
when not matched then 
	insert (student_id, majorcode) values(s.id, s.major);
	
GO


GRANT EXEC ON usp_DownloadStudents TO PUBLIC

GO



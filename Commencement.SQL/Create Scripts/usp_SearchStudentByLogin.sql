IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_SearchStudentByLogin')
	BEGIN
		DROP  Procedure  usp_SearchStudentByLogin
	END

GO

CREATE Procedure usp_SearchStudentByLogin
	(
		@login varchar(50),
		@term varchar(6)
	)

AS

	declare @tsql varchar(max)
	
	set @login = upper(@login)
	
	set @tsql = '
			select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name, shrlgpa_hours_earned, goremal_email_address, zgvlcfs_majr_code, zgvlcfs_coll_code, shrdgmr_degs_code, ''' + lower(@login)  + ''' loginid, shrttrm_astd_code_end_of_term
	from (	
		select * from openquery (sis, ''
		select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name, shrlgpa_hours_earned, email.goremal_email_address, curriculum.zgvlcfs_majr_code
			, curriculum.zgvlcfs_coll_code
			, shrdgmr_degs_code
			, shrttrm_astd_code_end_of_term
		from spriden
			inner join shrlgpa on spriden_pidm = shrlgpa_pidm
			inner join wormoth on wormoth_pidm = spriden_pidm
			left outer join (
				select goremal_pidm, goremal_email_address
				from goremal
				where goremal_emal_code = ''''UCD''''
					and goremal_status_ind = ''''A''''
			) email on email.goremal_pidm = spriden_pidm
			inner join (
				select zgvlcfs_pidm, zgvlcfs_majr_code, zgvlcfs_coll_code
				from zgvlcfs
				where zgvlcfs_term_code_eff = '''''+@term+'''''
					and zgvlcfs_levl_code in (''''UG'''', ''''U2'''')
			) curriculum on curriculum.zgvlcfs_pidm = spriden_pidm
			left outer join shrdgmr on shrdgmr_pidm = spriden_pidm and shrdgmr_term_code_sturec = '''''+@term+'''''
			left outer join shrttrm on shrttrm_pidm = spriden_pidm
		where spriden_change_ind is null
			and wormoth_login_id = ''''' + @login + '''''
			and shrlgpa_gpa_type_ind = ''''O''''
			and shrlgpa_levl_code in (''''UG'''', ''''U2'''')
			and shrlgpa_hours_earned in ( 
				select max(shrlgpa_hours_earned) 
				from shrlgpa ishrlgpa 
				where shrlgpa.shrlgpa_pidm = ishrlgpa.shrlgpa_pidm )
			and shrttrm_term_code = (select max(stvterm_code) from stvterm where stvterm_end_date < sysdate and stvterm_trmt_code = ''''Q'''')
				'')) student
	'

	exec (@tsql)
	
	




GO

GRANT EXEC ON usp_SearchStudentByLogin TO PUBLIC

GO


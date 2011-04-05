IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_SearchStudent')
	BEGIN
		DROP  Procedure  usp_SearchStudent
	END

GO

CREATE Procedure usp_SearchStudent
	(
		@studentid varchar(9)
	)

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
		Email varchar(100),
		EarnedUnits decimal(6,3),
		CurrentUnits decimal(6,3),
		major varchar(4),	
		lastTerm varchar(6),
		astd varchar(2),
		LoginId varchar(50)
	)


	declare @tsql varchar(max)
	
	set @tsql = '
		insert into #Students
		select pidm, studentid
			 , firstname, mi, lastname
			 , email, earnedunits, currentunits
			 , major, lastterm, astd, loginid
			from openquery (sis, ''
			select spriden_pidm as pidm, spriden_id as studentId
			, spriden_first_name  as firstName, spriden_mi as mi, spriden_last_name as lastName
			, email.goremal_email_address as email
			, shrlgpa_hours_earned earnedUnits, 0 as currentUnits
			, zgvlcfs_majr_code as major
			, zgvlcfs_term_code_eff as lastTerm
			, shrttrm_astd_code_end_of_term as astd
			, lower(wormoth_login_id) as loginid
		from spriden
			inner join zgvlcfs on spriden_pidm = zgvlcfs_pidm
			inner join wormoth on spriden_pidm = wormoth_pidm
			inner join shrlgpa on spriden_pidm = shrlgpa_pidm
			left outer join shrttrm on spriden_pidm = shrttrm_pidm
			left outer join (
				select goremal_pidm, goremal_email_address
				from goremal
				where goremal_emal_code = ''''UCD''''
				  and goremal_status_ind = ''''A''''
			) email on email.goremal_pidm = spriden_pidm
		where spriden_id = ''''' + @studentid + '''''
		  and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
		  and spriden_change_ind is null
		  and shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''UG''''
		  and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where ishrttrm.shrttrm_pidm = shrttrm.shrttrm_pidm and shrttrm_astd_code_end_of_term is not null )
		'')
	'
	exec(@tsql)


	if not exists (select * from #Students)
	begin

		set @tsql = '
			insert into #Students
			select pidm, studentid
				 , firstname, mi, lastname
				 , email, earnedunits, currentunits
				 , major, lastterm, astd, loginid
				from openquery (sis, ''
				select spriden_pidm as pidm, spriden_id as studentId
				, spriden_first_name  as firstName, spriden_mi as mi, spriden_last_name as lastName
				, email.goremal_email_address as email
				, shrlgpa_hours_earned earnedUnits, 0 as currentUnits
				, zgvlcfs_majr_code as major
				, zgvlcfs_term_code_eff as lastTerm
				, shrttrm_astd_code_end_of_term as astd
				, lower(wormoth_login_id) as loginid
			from spriden
				inner join zgvlcfs on spriden_pidm = zgvlcfs_pidm
				inner join wormoth on spriden_pidm = wormoth_pidm
				inner join shrlgpa on spriden_pidm = shrlgpa_pidm
				left outer join shrttrm on spriden_pidm = shrttrm_pidm
				left outer join (
					select goremal_pidm, goremal_email_address
					from goremal
					where goremal_emal_code = ''''UCD''''
					  and goremal_status_ind = ''''A''''
				) email on email.goremal_pidm = spriden_pidm
			where spriden_id = ''''' + @studentid + '''''
			  and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
			  and spriden_change_ind is null
			  and shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''U2''''
			  and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where ishrttrm.shrttrm_pidm = shrttrm.shrttrm_pidm and shrttrm_astd_code_end_of_term is not null )
			'')
		'
		exec(@tsql)

	end

	select * from #students

	drop table #students

GO

GRANT EXEC ON usp_SearchStudent TO PUBLIC

GO


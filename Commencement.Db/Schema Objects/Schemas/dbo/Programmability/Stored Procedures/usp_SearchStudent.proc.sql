-- =============================================
-- Author:		Ken Taylor based on previous logic by Alan Lai.
-- Create date: February 22, 2017
-- Description:	Search for a student by student ID.
--
-- Usage:
/*
	 EXEC usp_SearchStudent @studentid = '998324966', @IsDebug = 0
*/
-- =============================================
CREATE Procedure usp_SearchStudent
	(
		@studentid varchar(9),
		@IsDebug bit = 0
	)

AS
	declare @tsql varchar(max)

	IF object_id('tempdb..#Students') IS NOT NULL
	BEGIN
		SELECT @tsql = '
		DROP TABLE #Students
'
		IF @IsDebug = 1
			PRINT @tsql
		ELSE
			EXEC(@tsql)
	END

	SELECT @tsql = '
	CREATE TABLE #Students
	(
		pidm varchar(8),
		studentId varchar(9),
		firstName varchar(50),
		mi varchar(50),
		lastName varchar(50),
		email varchar(100),
		earnedUnits decimal(6,3),
		currentUnits decimal(6,3),
		major varchar(4),	
		lastTerm varchar(6),
		astd varchar(2),
		loginId varchar(50)
		,sja	bit
	)
'

	SELECT @tsql += '
		insert into #Students
		select pidm, studentid
			 , firstname, mi, lastname
			 , email, earnedunits, currentunits
			 , major, lastterm, astd, loginid
			 , sja
			from openquery (sis, ''
			select spriden_pidm as pidm, spriden_id as studentId
			, spriden_first_name  as firstName, spriden_mi as mi, spriden_last_name as lastName
			, email.goremal_email_address as email
			, shrlgpa_hours_earned earnedUnits, 0 as currentUnits
			, zgvlcfs_majr_code as major
			, zgvlcfs_term_code_eff as lastTerm
			, shrttrm_astd_code_end_of_term as astd
			, lower(wormoth_login_id) as loginid
			, (case when sjaholds.sprhold_pidm is not null then 1 else 0 end) "sja"
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
			left outer join (
				select distinct sprhold_pidm
				from sprhold
				where sprhold_hldd_code in (''''BA'''', ''''BB'''', ''''RG'''')
				  and sprhold_from_date < sysdate
				  and sprhold_to_date > sysdate
			) sjaholds on sjaholds.sprhold_pidm = zgvlcfs_pidm
		where spriden_id = ''''' + @studentid + '''''
		  and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
		  and spriden_change_ind is null
		  and shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''UG''''
		  and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where ishrttrm.shrttrm_pidm = shrttrm.shrttrm_pidm and shrttrm_astd_code_end_of_term is not null )
		'')
	'

	SELECT @tsql += '
	if not exists (select * from #Students)
	begin
		insert into #Students
			select pidm, studentid
				 , firstname, mi, lastname
				 , email, earnedunits, currentunits
				 , major, lastterm, astd, loginid
				 , sja
				from openquery (sis, ''
				select spriden_pidm as pidm, spriden_id as studentId
				, spriden_first_name  as firstName, spriden_mi as mi, spriden_last_name as lastName
				, email.goremal_email_address as email
				, shrlgpa_hours_earned earnedUnits, 0 as currentUnits
				, zgvlcfs_majr_code as major
				, zgvlcfs_term_code_eff as lastTerm
				, shrttrm_astd_code_end_of_term as astd
				, lower(wormoth_login_id) as loginid
				, (case when sjaholds.sprhold_pidm is not null then 1 else 0 end) "sja"
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
				left outer join (
					select distinct sprhold_pidm
					from sprhold
					where sprhold_hldd_code in (''''BA'''', ''''BB'''', ''''RG'''')
					  and sprhold_from_date < sysdate
					  and sprhold_to_date > sysdate
				) sjaholds on sjaholds.sprhold_pidm = zgvlcfs_pidm
			where spriden_id = ''''' + @studentid + '''''
			  and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
			  and spriden_change_ind is null
			  and shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''U2''''
			  and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where ishrttrm.shrttrm_pidm = shrttrm.shrttrm_pidm and shrttrm_astd_code_end_of_term is not null )
			'')
		end
	'
	
	SELECT @tsql += '
	select * from #students

	drop table #students
'

	IF @IsDebug = 1
		PRINT @tsql
	ELSE
		exec(@tsql)
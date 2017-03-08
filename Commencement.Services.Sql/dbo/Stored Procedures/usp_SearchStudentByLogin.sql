
-- =============================================
-- Author:		Ken Taylor based on previous logic by Alan Lai.
-- Create date: February 22, 2017
-- Description:	Search for a student by login.
--
-- Usage:
/*
	 EXEC usp_SearchStudentByLogin @login = 'ikababse', @IsDebug = 0
*/
-- =============================================
CREATE Procedure usp_SearchStudentByLogin
	(
		@login varchar(50),
		@IsDebug bit = 0
	)

AS
	declare @tsql varchar(max)
	
	set @login = upper(@login)
	
	set @tsql = '
		select CONVERT(varchar(8), pidm) pidm, studentId
			 , firstName, mi, lastName
			 , email, CONVERT(decimal(6,3), earnedUnits) earnedUnits, CONVERT(decimal(6,3), currentUnits) currentUnits
			 , major, lastTerm, astd, loginId, CONVERT(bit,sja) sja from openquery(sis, ''
			select spriden_pidm as "pidm", spriden_id as "studentId"
			, spriden_first_name  as "firstName", spriden_mi as "mi", spriden_last_name as "lastName"
			, email.goremal_email_address as "email"
			, shrlgpa_hours_earned "earnedUnits", 0 as "currentUnits"
			, zgvlcfs_majr_code as "major"
			, zgvlcfs_term_code_eff as "lastTerm"
			, shrttrm_astd_code_end_of_term as "astd"
			, lower(wormoth_login_id) as "loginId"
			, (case when sjaholds.sprhold_pidm is not null then 1 else 0 end) "sja"
		from wormoth
			inner join zgvlcfs on wormoth_pidm = zgvlcfs_pidm
			inner join spriden on wormoth_pidm = spriden_pidm
			inner join shrlgpa on wormoth_pidm = shrlgpa_pidm
			left outer join shrttrm on wormoth_pidm = shrttrm_pidm
			left outer join (
				select goremal_pidm, goremal_email_address
				from goremal
				where goremal_emal_code = ''''UCD''''
					and goremal_status_ind = ''''A''''
			) email on email.goremal_pidm = wormoth_pidm
			left outer join (
				select distinct sprhold_pidm
				from sprhold
				where sprhold_hldd_code in (''''BA'''', ''''BB'''', ''''RG'''')
				  and sprhold_from_date < sysdate
				  and sprhold_to_date > sysdate
			) sjaholds on sjaholds.sprhold_pidm = zgvlcfs_pidm
		where wormoth_login_id = ''''' + @login + '''''
		  and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
		  and spriden_change_ind is null
		  and shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''UG''''
		  and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where ishrttrm.shrttrm_pidm = shrttrm.shrttrm_pidm and shrttrm_astd_code_end_of_term is not null )
		'')
	'

	--set @tsql = '
	--	select * from openquery(sis, ''
	--		select spriden_pidm as pidm, spriden_id as studentId
	--			, spriden_first_name as firstName, spriden_mi as mi, spriden_last_name as lastName
	--			, email.goremal_email_address as email
	--			, earnedunits.units as earnedunits
	--			, 0 as currentunits
	--			, zgvlcfs_majr_code as major
	--			, zgvlcfs_term_code_eff as lastTerm
	--			, shrttrm_astd_code_end_of_term as astd
	--			, lower(wormoth_login_id) as loginid
	--		from wormoth 
	--			inner join zgvlcfs on wormoth_pidm = zgvlcfs_pidm
	--			inner join spriden on wormoth_pidm = spriden_pidm
	--			left outer join (
	--				select shrttrm_pidm as pidm, shrttrm_astd_code_end_of_term
	--				from shrttrm
	--				where shrttrm_term_code in (select max(shrttrm_term_code) from shrttrm ishrttrm
	--											where ishrttrm.shrttrm_Pidm = shrttrm.shrttrm_pidm)
	--			) astd on astd.pidm = wormoth_pidm
	--			left outer join (
	--				select goremal_pidm, goremal_email_address
	--				from goremal
	--				where goremal_emal_code = ''''UCD''''
	--					and goremal_status_ind = ''''A''''
	--			) email on email.goremal_pidm = wormoth_pidm
	--			inner join (
	--				select shrlgpa_pidm as pidm, shrlgpa_hours_earned units
	--				from shrlgpa
	--				where shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''UG''''
	--			) EarnedUnits on EarnedUnits.pidm = zgvlcfs_pidm
	--		where wormoth_login_id = ''''' + @login + '''''
	--			and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
	--			and spriden_change_ind is null
	--	'')'

	IF @IsDebug = 1
		PRINT @tsql
	ELSE
		exec(@tsql)
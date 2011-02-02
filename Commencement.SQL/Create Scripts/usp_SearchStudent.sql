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

	declare @tsql varchar(max)
	
	set @tsql = '
		select * from openquery (sis, ''
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

	--set @tsql = '
	--	select * from openquery(sis, ''
	--		select spriden_pidm as pidm, spriden_id as studentId
	--			 , spriden_first_name as firstname, spriden_mi as mi, spriden_last_name as lastName
	--			 , email.goremal_email_address as email
	--			 , earnedunits.units as earnedUnits
	--			 , currentunits.units as currentUnits
	--			 , zgvlcfs_majr_code as major, max(zgvlcfs_term_code_eff) as lastTerm
	--			 , shrttrm_astd_code_end_of_term as astd
	--			 , lower(wormoth_login_id) as LoginId
	--		from spriden
	--			inner join zgvlcfs on zgvlcfs_pidm = spriden_pidm
	--			inner join wormoth on wormoth_pidm = spriden_pidm
	--			left outer join (
	--				select goremal_pidm, goremal_email_address
	--				from goremal
	--				where goremal_emal_code = ''''UCD''''
	--					and goremal_status_ind = ''''A''''
	--			) email on email.goremal_pidm = spriden_pidm
	--			inner join (
	--				select shrlgpa_pidm as pidm, shrlgpa_hours_earned units
	--				from shrlgpa
	--				where shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''UG''''
	--			) EarnedUnits on EarnedUnits.pidm = zgvlcfs_pidm
	--			left outer join (
	--				select sfrstcr_pidm as pidm, sum(sfrstcr_credit_hr) units
	--				from sfrstcr
	--					left join shrtckn on sfrstcr_pidm = shrtckn_pidm 
	--									 and sfrstcr_term_code = shrtckn_term_code
	--									 and sfrstcr_crn = shrtckn_crn
	--				where shrtckn_pidm is null
	--				group by sfrstcr_pidm    
	--			) CurrentUnits on CurrentUnits.pidm = spriden_pidm
	--			left outer join (
	--				select shrttrm_pidm as pidm, shrttrm_astd_code_end_of_term
	--				from shrttrm
	--				where shrttrm_term_code in (select max(shrttrm_term_code) from shrttrm ishrttrm
	--											where ishrttrm.shrttrm_Pidm = shrttrm.shrttrm_pidm)
	--			) astd on astd.pidm = spriden_pidm
	--		where spriden_id = '''''+@studentid+'''''
	--		  and spriden_change_ind is null
	--		group by spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name, email.goremal_email_address, earnedunits.units, currentunits.units, zgvlcfs_majr_code, shrttrm_astd_code_end_of_term, wormoth_login_id
	--	'')'

exec(@tsql)


GO

GRANT EXEC ON usp_SearchStudent TO PUBLIC

GO


﻿IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_SearchStudent')
	BEGIN
		DROP  Procedure  usp_SearchStudent
	END

GO

CREATE Procedure usp_SearchStudent

	(
		@studentid varchar(9),
		@term varchar(6)
	)


AS

declare @tsql varchar(max)

set @tsql = 'select * from openquery (sis, ''
	select spriden_pidm, spriden_id, spriden_first_name, spriden_last_name, shrlgpa_hours_earned, email.goremal_email_address, curriculum.zgvlcfs_majr_code
		, curriculum.zgvlcfs_coll_code
		, shrdgmr_degs_code
	from spriden
		inner join shrlgpa on spriden_pidm = shrlgpa_pidm
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
				and zgvlcfs_coll_code = ''''AE''''
		) curriculum on curriculum.zgvlcfs_pidm = spriden_pidm
		left outer join shrdgmr on shrdgmr_pidm = spriden_pidm and shrdgmr_term_code_sturec = '''''+@term+'''''
	where spriden_change_ind is null
		and shrlgpa_gpa_type_ind = ''''O''''
		and shrlgpa_levl_code in (''''UG'''', ''''U2'''')
		and shrlgpa_hours_earned in ( select max(shrlgpa_hours_earned) from shrlgpa ishrlgpa where shrlgpa.shrlgpa_pidm = ishrlgpa.shrlgpa_pidm )
        and spriden_id = '''''+@studentid+'''''
        '')
        '
        
exec (@tsql)        


GO

GRANT EXEC ON usp_SearchStudent TO PUBLIC

GO



-- =============================================
-- Author:		Scott Kirkland
-- Create date: 8/31/2016
-- Description:	Downloads all students for a given term and credit amount
-- Usage:
/*

	USE COMMENCEMENT
	GO

	EXEC [dbo].[usp_DownloadStudents] @minUnits = 120, @IsDebug = 1

*/
-- Modifications:
--	20170307 by srk: Revised term statements for find the last 10 terms, basically 1 year's
--		worth of terms because we were missing students from a quarter that hadn't started yet.
--	20170307 by kjt: Revised duplicate login selection logic to only check for active students.
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadStudents] 
	-- Add the parameters for the stored procedure here
	@minUnits int = 135,
	@IsDebug bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @tsql varchar(max)

	declare @minTerm varchar(6)
	declare @maxTerm varchar(6)

	declare @terms table (minTerm varchar(6), maxTerm varchar(6))

	-- get the 10 terms that have started 3 months from now
	insert into @terms 
	select min(term) minTerm, max(term) maxTerm from openquery(sis, '
												select stvterm_code term, stvterm_end_date, stvterm_start_date
												from stvterm
												where stvterm_start_date <= (sysdate + 90)
												and stvterm_code <> 999999
												and rownum <= 10
												order by stvterm_end_date desc
											')

	select @minTerm = minTerm from @terms
	select @maxTerm = maxTerm from @terms

	-- set @minUnits = (select min(PetitionThreshold) from Ceremonies where termcode = @term)    

	set @tsql = '
		TRUNCATE TABLE Commencement.dbo.Students

		insert into Commencement.dbo.Students
		select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name
			, earnedunits, currentunits, goremal_email_address, loginid, shrttrm_astd_code_end_of_term
			, sja
			, zgvlcfs_majr_code
		from openquery(sis, ''
			select distinct spriden_pidm, spriden_id, 
			(CASE 
				WHEN ZPVPREF_PREF_FIRST_NAME IS NOT NULL
				THEN ZPVPREF_PREF_FIRST_NAME 
				ELSE SPRIDEN_FIRST_NAME 
			END) spriden_first_name,
			spriden_mi, spriden_last_name
				, EarnedUnits.shrlgpa_hours_earned as EarnedUnits
				, 0 as CurrentUnits
				, email.goremal_email_address
				, lower(wormoth_login_id) loginId
				, shrttrm_astd_code_end_of_term
				, (case when sjaholds.sprhold_pidm is not null then 1 else 0 end) sja
				, zgvlcfs_majr_code
			from zgvlcfs
				inner join spriden on spriden_pidm = zgvlcfs_pidm
				LEFT OUTER JOIN ZPVPREF ON zgvlcfs_pidm = ZPVPREF_PIDM 
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
					join (
						select t.wormoth_pidm, max(t.wormoth_activity_date) as maxsubkey 
						from wormoth t 
						where t.wormoth_acct_type = ''''Z'''' AND t.wormoth_acct_status = ''''A''''
						group by t.wormoth_pidm 
					) b on b.maxsubkey = a.wormoth_activity_date and a.wormoth_pidm = b.wormoth_pidm
					where wormoth_acct_type = ''''Z'''' and wormoth_acct_status = ''''A''''
				) wormoth on wormoth_pidm = zgvlcfs_pidm
				left outer join shrttrm on shrttrm_pidm = zgvlcfs_pidm
				left outer join (
					select distinct sprhold_pidm
					from sprhold
					where sprhold_hldd_code in (''''RG'''')
					  and sprhold_from_date < sysdate
					  and sprhold_to_date > sysdate
				) sjaholds on sjaholds.sprhold_pidm = zgvlcfs_pidm
			where spriden_change_ind is null
				and zgvlcfs_term_code_eff between '''''+@minTerm+''''' and '''''+@maxTerm+'''''
				and EarnedUnits.shrlgpa_hours_earned > ' + CAST(@minUnits as varchar(6)) + '
				and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where shrttrm.shrttrm_pidm = ishrttrm.shrttrm_pidm )
				and earnedUnits.shrlgpa_gpa_type_ind = ''''O''''
				and earnedUnits.shrlgpa_levL_code = ''''UG''''
		'')
	'

	IF @IsDebug = 1
		PRINT @tsql
	ELSE
		exec (@tsql)

END
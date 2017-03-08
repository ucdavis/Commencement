
-- =============================================
-- Author:		Scott Kirkland
-- Create date: 8/31/2016
-- Description:	Downloads all students for a given term and credit amount
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadStudents_20170307_bak] 
	-- Add the parameters for the stored procedure here
	@minUnits int = 120
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @tsql varchar(max)
declare @sisterm varchar(6)

--select @sisterm = term from openquery(sis, '
--											select min(stvterm_code) term
--											from stvterm
--											where stvterm_end_date > sysdate
--											  and stvterm_trmt_code = ''Q''
--										')

Set @sisterm = '201703'

-- set @minUnits = (select min(PetitionThreshold) from Ceremonies where termcode = @term)    

TRUNCATE TABLE Commencement.dbo.Students

set @tsql = '
	insert into Commencement.dbo.Students
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

END
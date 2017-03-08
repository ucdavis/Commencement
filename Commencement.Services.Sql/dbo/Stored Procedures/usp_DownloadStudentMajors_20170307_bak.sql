


-- =============================================
-- Author:		Scott Kirkland
-- Create date: 8/31/2016
-- Description:	Downloads all majors and students in those majors for a given term
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadStudentMajors_20170307_bak] 

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

TRUNCATE TABLE Commencement.dbo.StudentMajors

set @tsql = '
	insert into Commencement.dbo.StudentMajors
	select zgvlcfs_pidm, zgvlcfs_majr_code from openquery (sis, '' 
	select zgvlcfs_pidm, zgvlcfs_majr_code from zgvlcfs 
	 where zgvlcfs_levl_code in (''''UG'''', ''''U2'''')
	 and zgvlcfs_term_code_eff = '''''+@sisterm+'''''
	'')
'

exec (@tsql)

END
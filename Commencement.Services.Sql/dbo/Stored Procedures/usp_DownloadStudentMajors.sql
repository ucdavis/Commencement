



-- =============================================
-- Author:		Scott Kirkland
-- Create date: 8/31/2016
-- Description:	Downloads all majors and students in those majors for a given term
-- Usage: 
/*

EXEC usp_DownloadStudentMajors

*/
-- =============================================
CREATE PROCEDURE [dbo].[usp_DownloadStudentMajors] 

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

TRUNCATE TABLE Commencement.dbo.StudentMajors

set @tsql = '
	insert into Commencement.dbo.StudentMajors
	select zgvlcfs_pidm, zgvlcfs_majr_code from openquery (sis, '' 
	select distinct zgvlcfs_pidm, zgvlcfs_majr_code from zgvlcfs 
	 where zgvlcfs_levl_code in (''''UG'''', ''''U2'''')
	and zgvlcfs_term_code_eff between '''''+@minTerm+''''' and '''''+@maxTerm+'''''
	'')
'

exec (@tsql)

END
CREATE VIEW [dbo].[vTermCodes]
AS
SELECT     Students.dbo.TermCodes.id, Students.dbo.TermCodes.Description, Students.dbo.TermCodes.StartDate, Students.dbo.TermCodes.EndDate, Students.dbo.TermCodes.TypeCode
FROM         Students.dbo.TermCodes
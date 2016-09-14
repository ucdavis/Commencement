USE [Commencement]
GO

INSERT INTO [dbo].[AllTermCodes]
SELECT id, Description, StartDate, EndDate, TypeCode
FROM Students.dbo.TermCodes
GO
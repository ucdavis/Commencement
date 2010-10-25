IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_SpecialNeedsReport')
    BEGIN
        DROP  Procedure  usp_SpecialNeedsReport
    END

GO

CREATE PROCEDURE [dbo].[usp_SpecialNeedsReport]
    @term varchar(6),
    @userid int
AS
    
SELECT     Students.LastName, Students.FirstName, Students.StudentId, vMajors.Name AS Major, Students.Email AS PrimaryEmail, Registrations.Email AS SecondaryEmail, 
                      Ceremonies.DateTime AS CeremonyTime, Registrations.Comments AS SpecialNeeds, TermCodes.Name AS Term,
                      ceremonies.id
FROM         Registrations INNER JOIN
                      Students ON Students.Id = Registrations.Student_Id INNER JOIN
                      Ceremonies ON Ceremonies.id = Registrations.CeremonyId INNER JOIN
                      vMajors ON vMajors.id = Registrations.MajorCode INNER JOIN
                      TermCodes ON Ceremonies.TermCode = TermCodes.id
WHERE   (Students.TermCode = @term) 
	AND (Registrations.Comments IS NOT NULL) 
	AND (LEN(Registrations.Comments) > 0) 
	AND (Registrations.SJABlock = 0)
	AND (Registrations.Cancelled = 0)
	and Ceremonies.id in (select CeremonyId from Ceremonies
								inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
							where UserId = @userid
								and TermCode = @term)

RETURN 0
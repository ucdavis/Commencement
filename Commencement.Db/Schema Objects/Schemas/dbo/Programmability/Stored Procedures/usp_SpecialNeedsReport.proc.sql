
CREATE PROCEDURE [dbo].[usp_SpecialNeedsReport]
    @term varchar(6),
    @userid int
AS

/*
	Returns a list of students with special needs marked
*/
    
--SELECT     Students.LastName, Students.FirstName, Students.StudentId, Majors.Name AS Major, Students.Email AS PrimaryEmail, Registrations.Email AS SecondaryEmail, 
--                      Ceremonies.DateTime AS CeremonyTime, Registrations.Comments AS SpecialNeeds, TermCodes.Name AS Term,
--                      ceremonies.id
--FROM         Registrations INNER JOIN
--                      Students ON Students.Id = Registrations.Student_Id INNER JOIN
--                      Ceremonies ON Ceremonies.id = Registrations.CeremonyId INNER JOIN
--                      Majors ON Majors.id = Registrations.MajorCode INNER JOIN
--                      TermCodes ON Ceremonies.TermCode = TermCodes.id
--WHERE   (Students.TermCode = @term) 
--	AND (Registrations.Comments IS NOT NULL) 
--	AND (LEN(Registrations.Comments) > 0) 
--	AND (Registrations.SJABlock = 0)
--	AND (Registrations.Cancelled = 0)
--	and Ceremonies.id in (select CeremonyId from Ceremonies
--								inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--							where UserId = @userid
--								and TermCode = @term)

select students.LastName, students.FirstName, students.StudentId
	, rp.MajorCode as Major
	, students.Email as PrimaryEmail
	, reg.Email as SecondaryEmail
	, ceremonies.DateTime as CeremonyTime
	, dbo.udf_GetSpecialNeedsCSV(reg.id) as SpecialNeeds
	, TermCodes.Name as Term
	, Ceremonies.id
from Registrations reg
	inner join Students on students.Id = reg.Student_Id
	inner join RegistrationParticipations rp on rp.RegistrationId = reg.id
	inner join Ceremonies on rp.CeremonyId = Ceremonies.id
	inner join TermCodes on Ceremonies.TermCode = termcodes.id
where students.TermCode = @term
  and students.SJABlock = 0
  and rp.Cancelled = 0
  and rp.CeremonyId in ( select CeremonyId from CeremonyEditors 
									inner join Ceremonies on CeremonyEditors.CeremonyId = Ceremonies.id
						 where UserId = @userId
						   and Ceremonies.TermCode = @term
					    )
  and dbo.udf_GetSpecialNeedsCSV(reg.id) is not null					    
order by Ceremonies.DateTime, students.LastName

RETURN 0
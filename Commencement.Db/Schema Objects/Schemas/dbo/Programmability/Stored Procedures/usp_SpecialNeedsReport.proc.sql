
CREATE PROCEDURE [dbo].[usp_SpecialNeedsReport]
    @term varchar(6),
    @userid int
AS

/*
	Returns a list of students with special needs marked
*/
    
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
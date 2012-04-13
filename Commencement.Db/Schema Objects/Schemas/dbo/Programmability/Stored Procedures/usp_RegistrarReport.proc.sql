
CREATE PROCEDURE [dbo].[usp_RegistrarReport]
	@term varchar(6),
	@userId int
AS

/*
	Registrar requires a list of all participating students
*/

select stud.StudentId, stud.LastName, stud.FirstName, stud.MI, rp.MajorCode
from RegistrationParticipations rp
	inner join Registrations reg on rp.RegistrationId = reg.id
	inner join Students stud on reg.Student_Id = stud.Id
where stud.SJABlock = 0
  and rp.Cancelled = 0
  and rp.CeremonyId in ( select CeremonyId from CeremonyEditors 
									inner join Ceremonies on CeremonyEditors.CeremonyId = Ceremonies.id
						 where UserId = @userId
						   and Ceremonies.TermCode = @term
					    )
order by stud.LastName

RETURN 0
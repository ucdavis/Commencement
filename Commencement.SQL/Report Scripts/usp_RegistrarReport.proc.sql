IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_RegistrarReport')
	BEGIN
		DROP  Procedure  usp_RegistrarReport
	END

GO

CREATE PROCEDURE [dbo].[usp_RegistrarReport]
	@term varchar(6),
	@userId int
AS

/*
	Registrar requires a list of all participating students
*/

--select students.StudentId, students.LastName, students.FirstName, students.MI, Majors.Name Major
--from Registrations
--	inner join Students on Registrations.Student_Id = students.Id
--	inner join Majors on Registrations.MajorCode = Majors.id
--where Registrations.SJABlock = 0
--	and Registrations.Cancelled = 0
--	and Registrations.CeremonyId in (select CeremonyId from Ceremonies
--									inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--								where UserId = @userid
--									and TermCode = @term)
--order by students.lastname

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
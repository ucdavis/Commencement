IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_RegistrarReport')
	BEGIN
		DROP  Procedure  usp_RegistrarReport
	END

GO

CREATE PROCEDURE [dbo].[usp_RegistrarReport]
	@term varchar(6),
	@userId int
AS

select students.StudentId, students.LastName, students.FirstName, students.MI, vmajors.Name Major
from Registrations
	inner join Students on Registrations.Student_Id = students.Id
	inner join vMajors on Registrations.MajorCode = vmajors.id
where Registrations.SJABlock = 0
	and Registrations.Cancelled = 0
	and Registrations.CeremonyId in (select CeremonyId from Ceremonies
									inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
								where UserId = @userid
									and TermCode = @term)
order by students.lastname

RETURN 0
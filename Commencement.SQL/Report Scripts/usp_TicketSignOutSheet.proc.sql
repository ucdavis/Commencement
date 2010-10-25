IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_TicketSignOutSheet')
    BEGIN
        DROP  Procedure  usp_TicketSignOutSheet
    END

GO

CREATE PROCEDURE [dbo].[usp_TicketSignOutSheet]
	@term varchar(6),
	@userid int
AS
	
	select students.LastName, students.FirstName, students.StudentId, vmajors.Name Major
	 , Registrations.NumberTickets RegistrationTickets
	 , ISNULL(ExtraTicketPetitions.numbertickets, 0) ExtraTickets
	 , Registrations.CeremonyId		
	from Registrations
		left outer join ExtraTicketPetitions on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id and ExtraTicketPetitions.IsPending = 0 and ExtraTicketPetitions.IsApproved = 1
		inner join Students on Registrations.Student_Id = students.Id
		inner join vMajors on Registrations.MajorCode = vmajors.id
	where Students.TermCode = @term
		and Registrations.SJABlock = 0
		and Registrations.CeremonyId in 
			(select CeremonyId from Ceremonies
			inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
			where UserId = @userid
			and TermCode = @term)
	order by LastName


RETURN 0
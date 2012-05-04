
CREATE PROCEDURE [dbo].[usp_TicketSignOutSheet]
	@term varchar(6),
	@userid int
AS
	
	/*
		List of students who have decided to pickup their tickets
	*/

	select students.LastName, students.FirstName, students.StudentId, rp.MajorCode Major
		, rp.NumberTickets RegistrationTickets
		, ISNULL(etp.NumberTickets, 0) ExtraTickets
		, ISNULL(etp.NumberTicketsStreaming, 0) ExtraStreamingTickets
	from RegistrationParticipations rp
		inner join Registrations reg on rp.RegistrationId = reg.id
		inner join Students on students.Id = reg.Student_Id
		left outer join ExtraTicketPetitions etp on etp.id = rp.ExtraTicketPetitionId and etp.IsPending = 1 and etp.IsApproved = 1
	where students.TermCode = @term
	  and reg.MailTickets = 0
	  and students.SJABlock = 0
	  and rp.Cancelled = 0
	  and rp.CeremonyId in ( select CeremonyId from Ceremonies
								inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
							 where UserId = @userid
							   and TermCode = @term)

	--select students.LastName, students.FirstName, students.StudentId, Majors.Name Major
	-- , Registrations.NumberTickets RegistrationTickets
	-- , ISNULL(ExtraTicketPetitions.numbertickets, 0) ExtraTickets
	-- , Registrations.CeremonyId		
	--from Registrations
	--	left outer join ExtraTicketPetitions on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id and ExtraTicketPetitions.IsPending = 0 and ExtraTicketPetitions.IsApproved = 1
	--	inner join Students on Registrations.Student_Id = students.Id
	--	inner join Majors on Registrations.MajorCode = Majors.id
	--where Students.TermCode = @term
	--	and Registrations.MailTickets = 0
	--	and Registrations.SJABlock = 0
	--	and Registrations.Cancelled = 0
	--	and Registrations.CeremonyId in 
	--		(select CeremonyId from Ceremonies
	--		inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
	--		where UserId = @userid
	--		and TermCode = @term)
	--order by LastName


RETURN 0
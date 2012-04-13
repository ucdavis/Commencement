
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
		left outer join TicketDistributionMethods tdm on rp.TicketDistributionMethodId = tdm.id
		inner join Registrations reg on rp.RegistrationId = reg.id
		inner join Students on students.Id = reg.Student_Id
		left outer join ExtraTicketPetitions etp on etp.id = rp.ExtraTicketPetitionId and etp.IsPending = 1 and etp.IsApproved = 1
	where students.TermCode = @term
	  and tdm.Id = 'PU'
	  and students.SJABlock = 0
	  and rp.Cancelled = 0
	  and rp.CeremonyId in ( select CeremonyId from Ceremonies
								inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
							 where UserId = @userid
							   and TermCode = @term)

RETURN 0
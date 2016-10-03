CREATE PROCEDURE [dbo].[usp_TicketingByTerm]
	@term varchar(6)
AS
	select students.StudentId, students.LastName, students.FirstName
	, reg.email secondaryEmail, students.email primaryEmail
	, rp.NumberTickets RegistrationTickets
	, isnull(etp.NumberTickets, 0) ExtraTickets
	, rp.NumberTickets + isnull(etp.NumberTickets, 0) TotalTickets
	, c.datetime CeremonyTime
	, reg.TicketPassword
from RegistrationParticipations rp
	inner join Registrations reg on rp.RegistrationId = reg.id
	inner join Students on students.Id = reg.Student_Id
	left outer join ExtraTicketPetitions etp on etp.id = rp.ExtraTicketPetitionId and etp.IsPending = 0 and etp.IsApproved = 1
	inner join ceremonies c on rp.CeremonyId = c.id
where students.SJABlock = 0
	and rp.Cancelled = 0
	and c.termcode = @term
order by c.datetime, lastname
RETURN 0

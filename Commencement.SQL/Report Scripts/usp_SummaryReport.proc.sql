IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_SummaryReport')
    BEGIN
        DROP  Procedure  usp_SummaryReport
    END

GO

CREATE PROCEDURE [dbo].[usp_SummaryReport]
	@term varchar(6),
	@userid int
AS

select Ceremonies.id ceremonyid, Ceremonies.[DateTime], Ceremonies.TermCode, Ceremonies.TotalTickets totalceremonytickets
	, RegistrationTickets.NumberTickets RegistrationTickets
	, ISNULL(PendingExtraTickets.NumberTickets, 0) PendingExtraTickets
	, ISNULL(ApprovedExtraTickets.NumberTickets, 0) ApprovedExtraTickets
from Ceremonies
	left outer join (
		select sum(NumberTickets) NumberTickets, registrations.CeremonyId from Registrations
			inner join Students on Registrations.Student_Id = students.Id
		where Registrations.SJABlock = 0
			and Registrations.Cancelled = 0
			and students.TermCode = @term
		group by Registrations.CeremonyId
	) RegistrationTickets on RegistrationTickets.ceremonyid = ceremonies.id
	left outer join (
		select SUM(ExtraTicketPetitions.NumberTickets) NumberTickets, Registrations.CeremonyId from ExtraTicketPetitions
			inner join Registrations on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id
			inner join Students on Registrations.Student_Id = students.Id
		where Registrations.SJABlock = 0
			and Registrations.Cancelled = 0
			and students.TermCode = @term
			and ExtraTicketPetitions.IsPending = 1
		group by Registrations.CeremonyId
	) PendingExtraTickets on PendingExtraTickets.ceremonyid = ceremonies.id
	left outer join (
		select SUM(ExtraTicketPetitions.NumberTickets) NumberTickets, Registrations.CeremonyId from ExtraTicketPetitions
			inner join Registrations on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id
			inner join Students on Registrations.Student_Id = students.Id
		where Registrations.SJABlock = 0
			and Registrations.Cancelled = 0
			and students.TermCode = @term
			and ExtraTicketPetitions.IsPending = 0
			and ExtraTicketPetitions.IsApproved = 1
		group by Registrations.CeremonyId
	) ApprovedExtraTickets on ApprovedExtraTickets.ceremonyid = ceremonies.id	
where ceremonies.id in (select CeremonyId from Ceremonies
							inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
						where UserId = @userid
							and TermCode = @term)


RETURN 0
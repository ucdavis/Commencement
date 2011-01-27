IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_SummaryReport')
    BEGIN
        DROP  Procedure  usp_SummaryReport
    END

GO

CREATE PROCEDURE [dbo].[usp_SummaryReport]
	@term varchar(6),
	@userid int
AS

/*
	Provides a summary of all registrations
*/

select Ceremonies.id, Ceremonies.DateTime, Ceremonies.TermCode, Ceremonies.TotalTickets totalceremonytickets
	, RegistrationTickets.numberTickets RegistrationTickets
	, ISNULL(PendingExtraTickets.NumberTickets, 0) PendingExtraTickets
	, ISNULL(PendingExtraTickets.NumberStreamingTickets, 0) PendingStreamingExtraTickets
	, ISNULL(ApprovedExtraTickets.NumberTickets, 0) ApprovedExtraTickets
	, ISNULL(ApprovedExtraTickets.NumberStreamingTickets, 0) ApprovedStreamingExtraTickets
from ceremonies
	left outer join (
		select SUM(numbertickets) numberTickets, rp.CeremonyId
		from RegistrationParticipations rp
			inner join Registrations reg on reg.id = rp.RegistrationId
			inner join Students on students.Id = reg.Student_Id
		where students.SJABlock = 0
		  and rp.Cancelled = 0
		  and students.TermCode = @term
		group by rp.CeremonyId
	) RegistrationTickets on RegistrationTickets.CeremonyId = ceremonies.id
	left outer join (
		select SUM(etp.NumberTickets) NumberTickets, SUM(etp.NumberTicketsStreaming) NumberStreamingTickets
			, rp.CeremonyId
		from RegistrationParticipations rp
			inner join ExtraTicketPetitions etp on rp.ExtraTicketPetitionId = etp.id
			inner join Registrations reg on reg.id = rp.RegistrationId
			inner join Students on students.Id = reg.Student_Id
		where students.SJABlock = 0
		  and rp.Cancelled = 0
		  and students.TermCode = @term
		  and etp.IsPending = 1
		group by rp.CeremonyId			
	) PendingExtraTickets on PendingExtraTickets.CeremonyId = Ceremonies.id
	left outer join (
		select SUM(etp.NumberTickets) NumberTickets, SUM(etp.NumberTicketsStreaming) NumberStreamingTickets
			, rp.CeremonyId
		from RegistrationParticipations rp
			inner join ExtraTicketPetitions etp on rp.ExtraTicketPetitionId = etp.id
			inner join Registrations reg on reg.id = rp.RegistrationId
			inner join Students on students.Id = reg.Student_Id
		where students.SJABlock = 0
		  and rp.Cancelled = 0
		  and students.TermCode = @term
		  and etp.IsPending = 0
		  and etp.IsApproved = 1
		group by rp.CeremonyId			
	) ApprovedExtraTickets on PendingExtraTickets.CeremonyId = Ceremonies.id
where ceremonies.id in ( select CeremonyId from CeremonyEditors 
									inner join Ceremonies on CeremonyEditors.CeremonyId = Ceremonies.id
						 where UserId = @userId
						   and Ceremonies.TermCode = @term
					    )	

--select Ceremonies.id ceremonyid, Ceremonies.[DateTime], Ceremonies.TermCode, Ceremonies.TotalTickets totalceremonytickets
--	, RegistrationTickets.NumberTickets RegistrationTickets
--	, ISNULL(PendingExtraTickets.NumberTickets, 0) PendingExtraTickets
--	, ISNULL(ApprovedExtraTickets.NumberTickets, 0) ApprovedExtraTickets
--from Ceremonies
--	left outer join (
--		select sum(NumberTickets) NumberTickets, registrations.CeremonyId from Registrations
--			inner join Students on Registrations.Student_Id = students.Id
--		where Registrations.SJABlock = 0
--			and Registrations.Cancelled = 0
--			and students.TermCode = @term
--		group by Registrations.CeremonyId
--	) RegistrationTickets on RegistrationTickets.ceremonyid = ceremonies.id
--	left outer join (
--		select SUM(ExtraTicketPetitions.NumberTickets) NumberTickets, Registrations.CeremonyId from ExtraTicketPetitions
--			inner join Registrations on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id
--			inner join Students on Registrations.Student_Id = students.Id
--		where Registrations.SJABlock = 0
--			and Registrations.Cancelled = 0
--			and students.TermCode = @term
--			and ExtraTicketPetitions.IsPending = 1
--		group by Registrations.CeremonyId
--	) PendingExtraTickets on PendingExtraTickets.ceremonyid = ceremonies.id
--	left outer join (
--		select SUM(ExtraTicketPetitions.NumberTickets) NumberTickets, Registrations.CeremonyId from ExtraTicketPetitions
--			inner join Registrations on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id
--			inner join Students on Registrations.Student_Id = students.Id
--		where Registrations.SJABlock = 0
--			and Registrations.Cancelled = 0
--			and students.TermCode = @term
--			and ExtraTicketPetitions.IsPending = 0
--			and ExtraTicketPetitions.IsApproved = 1
--		group by Registrations.CeremonyId
--	) ApprovedExtraTickets on ApprovedExtraTickets.ceremonyid = ceremonies.id	
--where ceremonies.id in (select CeremonyId from Ceremonies
--							inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--						where UserId = @userid
--							and TermCode = @term)


RETURN 0
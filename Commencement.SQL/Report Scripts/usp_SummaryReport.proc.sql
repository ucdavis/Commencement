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

select ceremonies.id, ceremonies.DateTime, ceremonies.TermCode
	, ceremonies.totaltickets totalceremonytickets
	, isnull(ceremonies.totalstreamingtickets, 0) TotalStreamingTickets
	, registrationtickets.tickets RegistrationTickets
	, isnull(pendingextratickets.tickets, 0) PendingExtraTickets
	, isnull(pendingextratickets.streamingtickets, 0) PendingStreamingExtraTickets
	, isnull(extratickets.tickets, 0) ApprovedExtraTickets
	, isnull(extratickets.streamingtickets, 0) ApprovedStreamingExtraTickets
from ceremonies
	left outer join (
		select sum(rp.numbertickets) tickets, rp.ceremonyid
		from registrationparticipations rp
			inner join registrations r on rp.registrationid = r.id
			inner join students s on r.student_id = s.id
		where rp.cancelled = 0
		  and s.sjablock = 0 and s.blocked = 0
		group by rp.ceremonyid
	) RegistrationTickets on RegistrationTickets.CeremonyId = ceremonies.id
	left outer join (
		select sum(etp.numbertickets) tickets, sum(etp.numberticketsstreaming) streamingtickets, ceremonyid
		from extraticketpetitions etp
			inner join registrationparticipations rp on rp.extraticketpetitionid = etp.id
		where etp.ispending = 0 and etp.isapproved = 1 and rp.cancelled = 0
		group by rp.ceremonyid
	) ExtraTickets on ExtraTickets.CeremonyId = ceremonies.id
	left outer join (
		select sum(isnull(etp.numbertickets, etp.numberticketsrequested)) tickets
			 , sum(isnull(etp.numberticketsstreaming, etp.numberticketsrequestedstreaming)) streamingtickets
			 , ceremonyid
		from extraticketpetitions etp
			inner join registrationparticipations rp on rp.extraticketpetitionid = etp.id
		where etp.ispending = 1
		group by rp.ceremonyid
	) PendingExtraTickets on PendingExtraTickets.CeremonyId = ceremonies.id
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
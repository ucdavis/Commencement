
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
		where etp.ispending = 1 and rp.Cancelled = 0
		group by rp.ceremonyid
	) PendingExtraTickets on PendingExtraTickets.CeremonyId = ceremonies.id
where ceremonies.id in ( select CeremonyId from CeremonyEditors 
									inner join Ceremonies on CeremonyEditors.CeremonyId = Ceremonies.id
						 where UserId = @userId
						   and Ceremonies.TermCode = @term
					    )	

RETURN 0
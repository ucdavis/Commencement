﻿CREATE PROCEDURE [dbo].[usp_TotalRegisteredByMajor]
	@term varchar(6),
	@userid int,
	@major varchar(4)
AS

select s.lastname, s.firstname, s.studentid, rp.majorcode major
	, r.address1, r.address2, r.city, r.[state], r.zip, s.email as PrimaryEmail
	, r.email as SecondaryEmail, rp.numbertickets
	, etp.numbertickets as ExtraTickets, etp.numberticketsstreaming as ExtraStreamingTickets
	, case
		when etp.numbertickets is null then rp.numbertickets
		else etp.numbertickets + rp.numbertickets
		end as totaltickets
		, s.termcode as term
		, case when r.mailtickets = 1 then 'Mail' else 'Pickup' end as DistributionMethod
	, rp.dateregistered
	, c.datetime as CeremonyTime
from registrationparticipations rp
	inner join registrations r on rp.registrationid = r.id
	inner join Students s on r.student_id = s.id
	left outer join extraticketpetitions etp on etp.id = rp.extraticketpetitionid and etp.ispending = 1 and etp.isapproved = 1
	inner join ceremonies c on c.id = rp.ceremonyid
where r.termcode = @term
  and rp.ceremonyid in (select ceremonyid from ceremonyeditors where userid = @userid)
  and s.sjablock = 0
  and rp.cancelled = 0
  and rp.majorcode = @major
order by s.lastname



RETURN 0
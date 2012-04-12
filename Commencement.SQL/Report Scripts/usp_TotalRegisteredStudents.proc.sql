CREATE PROCEDURE [dbo].[usp_TotalRegisteredStudents]
	@term varchar(6),
	@userid int
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
order by s.lastname


--SELECT     Students.LastName, Students.FirstName, Students.StudentId, Majors.Name AS Major, Registrations.Address1, Registrations.Address2, Registrations.City, 
--                      Registrations.State, Registrations.Zip, Students.Email AS PrimaryEmail, Registrations.Email AS SecondaryEmail, Registrations.NumberTickets, 
--                      ExtraTicketPetitions.NumberTickets AS ExtraTicketPetitions, Registrations.MailTickets, Ceremonies.DateTime AS CeremonyTime, 
--                      CASE 
--						WHEN ExtraTicketPetitions.NumberTickets IS NULL 
--						THEN Registrations.NumberTickets 
--						ELSE ExtraTicketPetitions.NumberTickets + Registrations.NumberTickets 
--					END AS Tickets, 
--					SUM(Registrations.NumberTickets) 
--                      AS RegistrationTickets, 
--					  SUM((CASE WHEN ((ExtraTicketPetitions.NumberTickets IS NULL)) 
--								THEN Registrations.NumberTickets 
--								ELSE Registrations.NumberTickets + ExtraTicketPetitions.NumberTickets END)) 
--								AS TotalTickets
--					, Students.TermCode, TermCodes.Name AS Term, CASE WHEN MailTickets = 1 THEN 'Mail' ELSE 'Pickup' END AS DistributionMethod,
--					  Registrations.DateRegistered
--FROM         Registrations INNER JOIN
--                      Students ON Students.Id = Registrations.Student_Id LEFT OUTER JOIN
--                      ExtraTicketPetitions ON ExtraTicketPetitions.id = Registrations.ExtraTicketPetitionId AND ExtraTicketPetitions.IsApproved = 1 AND 
--                      ExtraTicketPetitions.IsPending = 0 INNER JOIN
--                      Ceremonies ON Ceremonies.id = Registrations.CeremonyId INNER JOIN
--                      Majors ON Majors.id = Registrations.MajorCode INNER JOIN
--                      TermCodes ON Ceremonies.TermCode = TermCodes.id
--WHERE     (Registrations.SJABlock = 0) and registrations.cancelled = 0
--	and Ceremonies.id in 
--			(select CeremonyId from Ceremonies
--				inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--			where UserId = @userid
--				and TermCode = @term)
--GROUP BY Students.LastName, Students.FirstName, Students.StudentId, Majors.Name, Registrations.Address1, Registrations.Address2, Registrations.City, 
--                      Registrations.State, Registrations.Zip, Students.Email, Registrations.Email, Registrations.NumberTickets, ExtraTicketPetitions.NumberTickets, 
--                      Registrations.MailTickets, Ceremonies.DateTime, Students.TermCode, TermCodes.Name
--HAVING      (Students.TermCode = @term)

RETURN 0
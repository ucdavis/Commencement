IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'usp_TotalRegisteredStudents')
    BEGIN
        DROP  Procedure  usp_TotalRegisteredStudents
    END

GO

CREATE PROCEDURE [dbo].[usp_TotalRegisteredStudents]
	@term varchar(6),
	@userid int
AS

SELECT     Students.LastName, Students.FirstName, Students.StudentId, vMajors.Name AS Major, Registrations.Address1, Registrations.Address2, Registrations.City, 
                      Registrations.State, Registrations.Zip, Students.Email AS PrimaryEmail, Registrations.Email AS SecondaryEmail, Registrations.NumberTickets, 
                      ExtraTicketPetitions.NumberTickets AS ExtraTicketPetitions, Registrations.MailTickets, Ceremonies.DateTime AS CeremonyTime, 
                      CASE WHEN ExtraTicketPetitions.NumberTickets IS NULL 
                      THEN Registrations.NumberTickets ELSE ExtraTicketPetitions.NumberTickets + Registrations.NumberTickets END AS Tickets, SUM(Registrations.NumberTickets) 
                      AS RegistrationTickets, SUM((CASE WHEN ((ExtraTicketPetitions.NumberTickets IS NULL)) 
                      THEN Registrations.NumberTickets ELSE Registrations.NumberTickets + ExtraTicketPetitions.NumberTickets END)) AS TotalTickets, Students.TermCode, 
                      TermCodes.Name AS Term, CASE WHEN MailTickets = 1 THEN 'Mail' ELSE 'Pickup' END AS DistributionMethod
FROM         Registrations INNER JOIN
                      Students ON Students.Id = Registrations.Student_Id LEFT OUTER JOIN
                      ExtraTicketPetitions ON ExtraTicketPetitions.id = Registrations.ExtraTicketPetitionId AND ExtraTicketPetitions.IsApproved = 1 AND 
                      ExtraTicketPetitions.IsPending = 0 INNER JOIN
                      Ceremonies ON Ceremonies.id = Registrations.CeremonyId INNER JOIN
                      vMajors ON vMajors.id = Registrations.MajorCode INNER JOIN
                      TermCodes ON Ceremonies.TermCode = TermCodes.id
WHERE     (Registrations.SJABlock = 0)
	and Ceremonies.id in 
			(select CeremonyId from Ceremonies
				inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
			where UserId = @userid
				and TermCode = @term)
GROUP BY Students.LastName, Students.FirstName, Students.StudentId, vMajors.Name, Registrations.Address1, Registrations.Address2, Registrations.City, 
                      Registrations.State, Registrations.Zip, Students.Email, Registrations.Email, Registrations.NumberTickets, ExtraTicketPetitions.NumberTickets, 
                      Registrations.MailTickets, Ceremonies.DateTime, Students.TermCode, TermCodes.Name
HAVING      (Students.TermCode = @term)

RETURN 0
CREATE VIEW dbo.TotalWithPhoneticTermAndUserHardCoded
AS
SELECT        TOP (100) PERCENT s.LastName, s.FirstName, s.StudentId, CASE WHEN majors.ConsolidationCode IS NOT NULL 
                         THEN majors.ConsolidationCode ELSE rp.MajorCode END AS major, dbo.Majors.CollegeCode, r.Address1, r.Address2, r.City, r.State, r.Zip, s.Email AS PrimaryEmail, 
                         r.Email AS SecondaryEmail, rp.NumberTickets, etp.NumberTickets AS ExtraTickets, etp.NumberTicketsStreaming AS ExtraStreamingTickets, 
                         CASE WHEN etp.numbertickets IS NULL THEN rp.numbertickets ELSE etp.numbertickets + rp.numbertickets END AS totaltickets, s.TermCode AS term, 
                         CASE WHEN r.mailtickets = 1 THEN 'Mail' ELSE 'Pickup' END AS DistributionMethod, rp.DateRegistered, c.DateTime AS CeremonyTime, 
                         CASE WHEN r.GradTrack = 1 THEN 'Yes' ELSE 'No' END AS GradImages, r.Phonetic
FROM            dbo.RegistrationParticipations AS rp INNER JOIN
                         dbo.Registrations AS r ON rp.RegistrationId = r.id INNER JOIN
                         dbo.Students AS s ON r.Student_Id = s.Id LEFT OUTER JOIN
                         dbo.ExtraTicketPetitions AS etp ON etp.id = rp.ExtraTicketPetitionId AND etp.IsPending = 1 AND etp.IsApproved = 1 INNER JOIN
                         dbo.Ceremonies AS c ON c.id = rp.CeremonyId INNER JOIN
                         dbo.Majors ON dbo.Majors.id = rp.MajorCode
WHERE        (r.TermCode = 201503) AND (rp.CeremonyId IN
                             (SELECT        CeremonyId
                               FROM            dbo.CeremonyEditors
                               WHERE        (UserId = 102))) AND (s.SJABlock = 0) AND (rp.Cancelled = 0)
ORDER BY s.LastName
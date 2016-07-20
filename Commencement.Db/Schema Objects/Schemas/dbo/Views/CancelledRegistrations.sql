CREATE VIEW dbo.CancelledRegistrations
AS
SELECT        dbo.Students.Id, dbo.Students.StudentId, dbo.Students.FirstName, dbo.Students.MI, dbo.Students.LastName, dbo.Students.Email, 
                         dbo.RegistrationParticipations.CeremonyId, dbo.RegistrationParticipations.MajorCode, dbo.RegistrationParticipations.NumberTickets, 
                         dbo.RegistrationParticipations.DateRegistered, dbo.RegistrationParticipations.DateUpdated
FROM            dbo.Registrations INNER JOIN
                         dbo.RegistrationParticipations ON dbo.Registrations.id = dbo.RegistrationParticipations.RegistrationId INNER JOIN
                         dbo.Students ON dbo.Registrations.Student_Id = dbo.Students.Id
WHERE        (dbo.RegistrationParticipations.Cancelled = 1)
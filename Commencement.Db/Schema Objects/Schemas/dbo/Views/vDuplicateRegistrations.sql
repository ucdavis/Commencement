CREATE VIEW dbo.vDuplicateRegistrations
AS
SELECT        TOP (100) PERCENT COUNT(*) AS cnt, dbo.RegistrationParticipations.RegistrationId, dbo.RegistrationParticipations.CeremonyId, dbo.Students.StudentId
FROM            dbo.RegistrationParticipations INNER JOIN
                         dbo.Registrations ON dbo.RegistrationParticipations.RegistrationId = dbo.Registrations.id INNER JOIN
                         dbo.Students ON dbo.Registrations.Student_Id = dbo.Students.Id
GROUP BY dbo.RegistrationParticipations.RegistrationId, dbo.RegistrationParticipations.CeremonyId, dbo.Students.StudentId
HAVING        (COUNT(*) > 1)
ORDER BY cnt DESC
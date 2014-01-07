CREATE VIEW [dbo].[vPendingEmail]
	AS SELECT        dbo.EmailQueue.id, dbo.EmailQueue.Subject, dbo.EmailQueue.Body, CASE WHEN registrations.email IS NULL 
                         THEN students.email ELSE students.email + ';' + registrations.email END AS emails, dbo.EmailQueue.AttachmentId, dbo.EmailQueue.Immediate, 
                         dbo.Attachments.Contents, dbo.Attachments.ContentType, dbo.Attachments.FileName
FROM            dbo.EmailQueue INNER JOIN
                         dbo.Students ON dbo.EmailQueue.Student_Id = dbo.Students.Id INNER JOIN
                         dbo.Registrations ON dbo.EmailQueue.RegistrationId = dbo.Registrations.id LEFT OUTER JOIN
                         dbo.Attachments ON dbo.EmailQueue.AttachmentId = dbo.Attachments.Id
WHERE        (dbo.EmailQueue.Pending = 1)

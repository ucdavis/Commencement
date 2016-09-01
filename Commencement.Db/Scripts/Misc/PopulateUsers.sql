USE [Commencement]
GO

INSERT INTO [dbo].Users
SELECT LoginID AS id, Email, Email AS UserEmail, Phone, FirstName, LastName, 1 AS IsActive, UserID AS CatbertUserId
FROM Catbert3.dbo.Users
WHERE (Inactive = 0) AND (UserID IN
(SELECT p.UserID
FROM Catbert3.dbo.Permissions AS p INNER JOIN
Catbert3.dbo.Applications AS a ON p.ApplicationID = a.ApplicationID
WHERE (a.Name = 'Commencement') AND (p.Inactive = 0)))

GO
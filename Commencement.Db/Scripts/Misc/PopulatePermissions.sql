USE [Commencement]
GO

INSERT INTO [dbo].[Permissions]
SELECT dbo.Roles.Id AS RoleId, dbo.Users.Id AS UserId
FROM Catbert3.dbo.ApplicationUsers_V INNER JOIN
dbo.Roles ON Catbert3.dbo.ApplicationUsers_V.Role = dbo.Roles.Name INNER JOIN
dbo.Users ON Catbert3.dbo.ApplicationUsers_V.UserID = dbo.Users.CatbertUserId
WHERE (Catbert3.dbo.ApplicationUsers_V.Name = 'Commencement') AND (Catbert3.dbo.ApplicationUsers_V.Inactive = 0) 
GO
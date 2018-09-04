CREATE VIEW dbo.vUsers
AS
SELECT        CatbertUserId AS id, Id AS LoginID, Email, Phone, FirstName, LastName, IsActive
FROM            dbo.Users
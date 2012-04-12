CREATE VIEW dbo.vUsers
AS
SELECT        UserID AS id, LoginID, Email, Phone, FirstName, LastName, EmployeeID, SID, UserKey
FROM            Catbert3.dbo.Users
WHERE        (Inactive = 0) AND (UserID IN
                             (SELECT        p.UserID
                               FROM            Catbert3.dbo.Permissions AS p INNER JOIN
                                                         Catbert3.dbo.Applications AS a ON p.ApplicationID = a.ApplicationID
                               WHERE        (a.Name = 'Commencement') AND (p.Inactive = 0)))
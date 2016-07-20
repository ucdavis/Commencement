CREATE VIEW dbo.vDuplicateStudents
AS
SELECT        StudentId, TermCode, COUNT(*) AS Expr1, Login, DateAdded, DateUpdated
FROM            dbo.Students
GROUP BY StudentId, TermCode, Login, DateAdded, DateUpdated
HAVING        (COUNT(*) > 1) AND (TermCode = '201310')
CREATE VIEW [dbo].[vWaitlistSummary]
AS
SELECT     Subject, CourseNumber, Sequence, Title, Department, InstructorFirstName, InstructorLastName, Date, COUNT(PIDM) AS [Number Wait]
FROM         dbo.Waitlist
GROUP BY Subject, CourseNumber, Sequence, Title, Department, InstructorFirstName, InstructorLastName, Date
CREATE VIEW [dbo].[vWaitlist]
AS
SELECT     CRN, Termcode, Subject, CourseNumber, Sequence, Title, InstructorFirstName, InstructorLastName, Department, Date, PIDM, Major
FROM         dbo.Waitlist
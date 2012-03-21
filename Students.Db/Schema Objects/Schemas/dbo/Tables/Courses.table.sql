CREATE TABLE [dbo].[Courses] (
    [SubjectCode] VARCHAR (4)  NULL,
    [CourseNum]   VARCHAR (5)  NULL,
    [Name]        VARCHAR (50) NULL,
    [Dept]        VARCHAR (4)  NULL,
    [CRN]         VARCHAR (5)  NOT NULL,
    [TermCode]    CHAR (6)     NOT NULL
);


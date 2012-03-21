CREATE TABLE [dbo].[Waitlist] (
    [PIDM]                VARCHAR (8)  NOT NULL,
    [Termcode]            VARCHAR (6)  NOT NULL,
    [CRN]                 VARCHAR (5)  NOT NULL,
    [Major]               VARCHAR (4)  NULL,
    [Subject]             VARCHAR (4)  NULL,
    [CourseNumber]        VARCHAR (5)  NULL,
    [Sequence]            VARCHAR (3)  NULL,
    [Title]               VARCHAR (30) NULL,
    [Department]          VARCHAR (4)  NULL,
    [InstructorLastName]  VARCHAR (50) NULL,
    [InstructorFirstName] VARCHAR (50) NULL,
    [DateTaken]           DATETIME     NOT NULL,
    [Date]                AS           ((((CONVERT([varchar](5),datepart(month,[datetaken]),(0))+'/')+CONVERT([varchar](5),datepart(day,[datetaken]),(0)))+'/')+CONVERT([varchar](5),datepart(year,[datetaken]),(0)))
);


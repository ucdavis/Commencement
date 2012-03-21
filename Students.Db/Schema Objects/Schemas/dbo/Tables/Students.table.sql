CREATE TABLE [dbo].[Students] (
    [PIDM]             VARCHAR (8)   NOT NULL,
    [StudentID]        VARCHAR (9)   NULL,
    [FirstName]        VARCHAR (60)  NULL,
    [MI]               VARCHAR (60)  NULL,
    [LastName]         VARCHAR (60)  NULL,
    [Email]            VARCHAR (100) NULL,
    [LastUpdate]       DATETIME      NULL,
    [Active]           BIT           NULL,
    [Login]            VARCHAR (50)  NULL,
    [LastCourseUpdate] DATETIME      NULL
);


CREATE TABLE [dbo].[StudentCourses] (
    [PIDM]             VARCHAR (8)    NOT NULL,
    [Units]            DECIMAL (5, 3) NULL,
    [GradeCode]        VARCHAR (6)    NULL,
    [GradeModeCode]    VARCHAR (2)    NULL,
    [LevelCode]        CHAR (2)       NULL,
    [RegistrationCode] CHAR (2)       NULL,
    [CRN]              VARCHAR (5)    NOT NULL,
    [TermCode]         CHAR (6)       NOT NULL,
    [ErrorFlag]        VARCHAR (1)    NULL,
    [Message]          VARCHAR (200)  NULL,
    [Invalid]          BIT            NULL,
    [TcknSeqNo]        INT            NULL,
    [RepeatInd]        BIT            NULL,
    [InProgressInd]    BIT            NULL,
    [CampusCode]       VARCHAR (2)    NULL,
    [Id]               AS             (((([PIDM]+'|')+[TermCode])+'|')+[Crn]),
    [LastUpdate]       DATETIME       NULL
);


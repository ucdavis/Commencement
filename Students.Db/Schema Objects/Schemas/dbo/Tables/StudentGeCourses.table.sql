CREATE TABLE [dbo].[StudentGeCourses] (
    [id]                UNIQUEIDENTIFIER NOT NULL,
    [PIDM]              VARCHAR (8)      NOT NULL,
    [TermCode]          CHAR (6)         NULL,
    [CRN]               VARCHAR (5)      NULL,
    [GeVersion]         INT              NULL,
    [GeSubjectAreaCode] CHAR (1)         NULL,
    [Units]             DECIMAL (5, 3)   NULL,
    [CreditConditional] BIT              NULL,
    [GeReasonCode]      VARCHAR (6)      NULL,
    [DenyCode]          VARCHAR (2)      NULL,
    [DenyDate]          DATETIME         NULL,
    [Comments]          VARCHAR (30)     NULL,
    [RowId]             VARCHAR (50)     NULL
);


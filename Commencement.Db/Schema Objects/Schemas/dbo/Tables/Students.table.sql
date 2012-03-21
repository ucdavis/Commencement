CREATE TABLE [dbo].[Students] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Pidm]         VARCHAR (8)      NOT NULL,
    [StudentId]    VARCHAR (9)      NOT NULL,
    [FirstName]    VARCHAR (50)     NULL,
    [MI]           VARCHAR (50)     NULL,
    [LastName]     VARCHAR (50)     NULL,
    [EarnedUnits]  DECIMAL (6, 3)   NULL,
    [CurrentUnits] DECIMAL (6, 3)   NULL,
    [Email]        VARCHAR (100)    NULL,
    [Login]        VARCHAR (50)     NULL,
    [DateAdded]    DATETIME         NULL,
    [DateUpdated]  DATETIME         NULL,
    [TermCode]     VARCHAR (6)      NULL,
    [CeremonyId]   INT              NULL,
    [SJABlock]     BIT              NOT NULL,
    [Blocked]      BIT              NOT NULL,
    [AddedBy]      VARCHAR (50)     NULL
);


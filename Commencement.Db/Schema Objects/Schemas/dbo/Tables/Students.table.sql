CREATE TABLE [dbo].[Students] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_Students_Id] DEFAULT (newid()) NOT NULL,
    [Pidm]         VARCHAR (8)      NOT NULL,
    [StudentId]    VARCHAR (9)      NOT NULL,
    [FirstName]    VARCHAR (50)     NULL,
    [MI]           VARCHAR (50)     NULL,
    [LastName]     VARCHAR (50)     NULL,
    [EarnedUnits]  DECIMAL (6, 3)   NULL,
    [CurrentUnits] DECIMAL (6, 3)   NULL,
    [Email]        VARCHAR (100)    NULL,
    [Login]        VARCHAR (50)     NULL,
    [DateAdded]    DATETIME         CONSTRAINT [DF_Students_DatedAdded] DEFAULT (getdate()) NULL,
    [DateUpdated]  DATETIME         CONSTRAINT [DF_Students_DateUpdated] DEFAULT (getdate()) NULL,
    [TermCode]     VARCHAR (6)      NULL,
    [CeremonyId]   INT              NULL,
    [SJABlock]     BIT              CONSTRAINT [DF_Students_SJABlock] DEFAULT ((0)) NOT NULL,
    [Blocked]      BIT              CONSTRAINT [DF_Students_Removed] DEFAULT ((0)) NOT NULL,
    [AddedBy]      VARCHAR (50)     NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Students_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id])
);




GO
CREATE NONCLUSTERED INDEX [nci_idx_Students_StudentId]
    ON [dbo].[Students]([StudentId] ASC);


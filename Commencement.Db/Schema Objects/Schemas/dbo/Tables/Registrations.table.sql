CREATE TABLE [dbo].[Registrations] (
    [id]                INT              IDENTITY (1, 1) NOT NULL,
    [Student_Id]        UNIQUEIDENTIFIER NOT NULL,
    [Address1]          VARCHAR (200)    NOT NULL,
    [Address2]          VARCHAR (200)    NULL,
    [City]              VARCHAR (100)    NOT NULL,
    [State]             CHAR (2)         NOT NULL,
    [Zip]               VARCHAR (15)     NOT NULL,
    [Email]             VARCHAR (100)    NULL,
    [MailTickets]       BIT              CONSTRAINT [DF_Registrations_MailTickets] DEFAULT ((0)) NOT NULL,
    [TermCode]          VARCHAR (6)      NOT NULL,
    [GradTrack]         BIT              CONSTRAINT [DF_Registrations_GradTrack] DEFAULT ((0)) NOT NULL,
    [TicketPassword]    VARCHAR (50)     NULL,
    [Phonetic]          VARCHAR (150)    NULL,
    [CellNumberForText] VARCHAR (10)     NULL,
    [CellCarrier]       VARCHAR (25)     NULL,
    CONSTRAINT [PK_Registrations] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Registrations_Registrations] FOREIGN KEY ([id]) REFERENCES [dbo].[Registrations] ([id]),
    CONSTRAINT [FK_Registrations_States] FOREIGN KEY ([State]) REFERENCES [dbo].[States] ([Id]),
    CONSTRAINT [FK_Registrations_Students] FOREIGN KEY ([Student_Id]) REFERENCES [dbo].[Students] ([Id]),
    CONSTRAINT [FK_Registrations_TermCodes] FOREIGN KEY ([TermCode]) REFERENCES [dbo].[TermCodes] ([id])
);






GO
CREATE NONCLUSTERED INDEX [nci_Registrations_Student_Id]
    ON [dbo].[Registrations]([Student_Id] ASC);
GO


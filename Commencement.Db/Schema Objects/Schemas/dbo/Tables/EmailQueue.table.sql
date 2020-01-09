CREATE TABLE [dbo].[EmailQueue] (
    [id]                          INT              IDENTITY (1, 1) NOT NULL,
    [Student_Id]                  UNIQUEIDENTIFIER NOT NULL,
    [Created]                     DATETIME         NOT NULL,
    [Pending]                     BIT              CONSTRAINT [DF_EmailQueue_Pending] DEFAULT ((1)) NOT NULL,
    [SentDateTime]                DATETIME         NULL,
    [TemplateId]                  INT              NULL,
    [Subject]                     VARCHAR (100)    NOT NULL,
    [Body]                        VARCHAR (MAX)    NOT NULL,
    [Immediate]                   BIT              CONSTRAINT [DF_EmailQueue_Immediate] DEFAULT ((0)) NOT NULL,
    [RegistrationId]              INT              NULL,
    [RegistrationParticipationId] INT              NULL,
    [RegistrationPetitionId]      INT              NULL,
    [ExtraTicketPetitionId]       INT              NULL,
    [ErrorCode]                   INT              NULL,
    [AttachmentId]                INT              NULL,
    CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_EmailQueue_Attachments] FOREIGN KEY ([AttachmentId]) REFERENCES [dbo].[Attachments] ([Id]),
    CONSTRAINT [FK_EmailQueue_ExtraTicketPetitions] FOREIGN KEY ([ExtraTicketPetitionId]) REFERENCES [dbo].[ExtraTicketPetitions] ([id]),
    CONSTRAINT [FK_EmailQueue_RegistrationParticipations] FOREIGN KEY ([RegistrationParticipationId]) REFERENCES [dbo].[RegistrationParticipations] ([id]),
    CONSTRAINT [FK_EmailQueue_RegistrationPetitions] FOREIGN KEY ([RegistrationPetitionId]) REFERENCES [dbo].[RegistrationPetitions] ([id]),
    CONSTRAINT [FK_EmailQueue_Registrations] FOREIGN KEY ([RegistrationId]) REFERENCES [dbo].[Registrations] ([id]),
    CONSTRAINT [FK_EmailQueue_Students] FOREIGN KEY ([Student_Id]) REFERENCES [dbo].[Students] ([Id]),
    CONSTRAINT [FK_EmailQueue_Templates] FOREIGN KEY ([TemplateId]) REFERENCES [dbo].[Templates] ([id])
);








GO
CREATE NONCLUSTERED INDEX [nci_wi_EmailQueue_C00F412CE52B3AFF59024CF718AD2826]
    ON [dbo].[EmailQueue]([RegistrationPetitionId] ASC)
    INCLUDE([AttachmentId], [Body], [Created], [ExtraTicketPetitionId], [Immediate], [Pending], [RegistrationId], [RegistrationParticipationId], [SentDateTime], [Student_Id], [Subject], [TemplateId]);


GO
CREATE NONCLUSTERED INDEX [nci_idx_EmailQueue_Student_ID]
    ON [dbo].[EmailQueue]([Student_Id] ASC)
    INCLUDE([Created], [Pending], [SentDateTime], [TemplateId], [Subject], [Body], [Immediate], [RegistrationId], [RegistrationParticipationId], [RegistrationPetitionId], [ExtraTicketPetitionId], [ErrorCode], [AttachmentId]);


GO
CREATE NONCLUSTERED INDEX [IX_EmailQueue_Pending]
    ON [dbo].[EmailQueue]([Pending] DESC, [Immediate] ASC)
    INCLUDE([RegistrationId], [Student_Id]);




GO
CREATE NONCLUSTERED INDEX [nci_EmailQueue_RegistrationId]
    ON [dbo].[EmailQueue]([RegistrationId] ASC);


GO
CREATE NONCLUSTERED INDEX [nci_EmailQueue_IsPendingImmediateCvrRegId]
    ON [dbo].[EmailQueue]([Pending] DESC, [Immediate] ASC)
    INCLUDE([id], [Student_Id], [RegistrationId]);


GO
CREATE NONCLUSTERED INDEX [nci_EmailQueue_Id]
    ON [dbo].[EmailQueue]([id] ASC);


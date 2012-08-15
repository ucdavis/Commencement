CREATE TABLE [dbo].[EmailQueue] (
    [id]                          INT              IDENTITY (1, 1) NOT NULL,
    [Student_Id]                  UNIQUEIDENTIFIER NOT NULL,
    [Created]                     DATETIME         NOT NULL,
    [Pending]                     BIT              NOT NULL,
    [SentDateTime]                DATETIME         NULL,
    [TemplateId]                  INT              NULL,
    [Subject]                     VARCHAR (100)    NOT NULL,
    [Body]                        VARCHAR (MAX)    NOT NULL,
    [Immediate]                   BIT              NOT NULL,
    [RegistrationId]              INT              NULL,
    [RegistrationParticipationId] INT              NULL,
    [RegistrationPetitionId]      INT              NULL,
    [ExtraTicketPetitionId]       INT              NULL,
    [ErrorCode]                   INT              NULL, 
    [AttachmentId] INT NULL, 
    CONSTRAINT [FK_EmailQueue_Attachments] FOREIGN KEY ([AttachmentId]) REFERENCES [Attachments]([Id])
);




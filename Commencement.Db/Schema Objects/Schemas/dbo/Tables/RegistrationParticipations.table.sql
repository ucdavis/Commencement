CREATE TABLE [dbo].[RegistrationParticipations] (
    [id]                         INT         IDENTITY (1, 1) NOT NULL,
    [RegistrationId]             INT         NOT NULL,
    [MajorCode]                  VARCHAR (4) NOT NULL,
    [CeremonyId]                 INT         NOT NULL,
    [NumberTickets]              INT         NULL,
    [Cancelled]                  BIT         CONSTRAINT [DF_RegistrationParticipations_Cancelled] DEFAULT ((0)) NULL,
    [LabelPrinted]               BIT         CONSTRAINT [DF_RegistrationParticipations_LabelPrinted] DEFAULT ((0)) NULL,
    [ExtraTicketPetitionId]      INT         NULL,
    [DateRegistered]             DATETIME    CONSTRAINT [DF_RegistrationParticipations_DateRegistered] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]                DATETIME    NULL,
    [TicketDistributionMethodId] VARCHAR (2) NULL,
    [ExitSurvey]                 BIT         DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RegistrationParticipations] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_RegistrationParticipations_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]),
    CONSTRAINT [FK_RegistrationParticipations_ExtraTicketPetitions] FOREIGN KEY ([ExtraTicketPetitionId]) REFERENCES [dbo].[ExtraTicketPetitions] ([id]),
    CONSTRAINT [FK_RegistrationParticipations_Registrations] FOREIGN KEY ([RegistrationId]) REFERENCES [dbo].[Registrations] ([id])
);






GO
CREATE NONCLUSTERED INDEX [nci_wi_RegistrationParticipations_6FA92F4770B3E23ADC75D9C5D1CF483F]
    ON [dbo].[RegistrationParticipations]([RegistrationId] ASC)
    INCLUDE([Cancelled], [CeremonyId], [DateRegistered], [DateUpdated], [ExitSurvey], [ExtraTicketPetitionId], [LabelPrinted], [MajorCode], [NumberTickets], [TicketDistributionMethodId]);


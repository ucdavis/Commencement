CREATE TABLE [dbo].[TransferRequests] (
    [Id]                          INT           IDENTITY (1, 1) NOT NULL,
    [RegistrationParticipationId] INT           NOT NULL,
    [CeremonyId]                  INT           NOT NULL,
    [Reason]                      VARCHAR (200) NOT NULL,
    [DateRequested]               DATETIME      DEFAULT (getdate()) NOT NULL,
    [UserId]                      INT           NOT NULL,
    [Pending]                     BIT           DEFAULT ((1)) NOT NULL,
    [MajorCode]                   VARCHAR (4)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TransferRequest_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]),
    CONSTRAINT [FK_TransferRequest_RegistrationParticipations] FOREIGN KEY ([RegistrationParticipationId]) REFERENCES [dbo].[RegistrationParticipations] ([id]),
    CONSTRAINT [FK_TransferRequests_Majors] FOREIGN KEY ([MajorCode]) REFERENCES [dbo].[Majors] ([id])
);



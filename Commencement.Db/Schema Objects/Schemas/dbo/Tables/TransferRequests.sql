CREATE TABLE [dbo].[TransferRequests]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RegistrationParticipationId] INT NOT NULL, 
    [CeremonyId] INT NOT NULL, 
	[Reason] varchar(200) not null,
    [DateRequested] DATETIME NOT NULL DEFAULT getdate(), 
    [UserId] INT NOT NULL, 
    [Pending] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_TransferRequest_RegistrationParticipations] FOREIGN KEY ([RegistrationParticipationId]) REFERENCES [RegistrationParticipations]([Id]), 
    CONSTRAINT [FK_TransferRequest_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [Ceremonies]([Id])

)

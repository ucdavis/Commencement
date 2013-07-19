CREATE TABLE [dbo].[RegistrationSurveys]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RegistrationParticipationId] INT NULL, 
    [CeremonyId] INT NOT NULL, 
    [Completed] DATETIME NOT NULL DEFAULT (getdate()), 
    [SurveyId] INT NOT NULL, 
    [RegistrationPetitionId] INT NULL, 
    CONSTRAINT [FK_SurveyRegistration_RegistrationParticipations] FOREIGN KEY ([RegistrationParticipationId]) REFERENCES [RegistrationParticipations]([Id]), 
    CONSTRAINT [FK_SurveyRegistration_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [Ceremonies]([Id]), 
    CONSTRAINT [FK_SurveyRegistration_Surveys] FOREIGN KEY ([SurveyId]) REFERENCES [Surveys]([Id]), 
    CONSTRAINT [FK_RegistrationSurveys_RegistrationPetitions] FOREIGN KEY ([RegistrationPetitionId]) REFERENCES [RegistrationPetitions]([id]),
)

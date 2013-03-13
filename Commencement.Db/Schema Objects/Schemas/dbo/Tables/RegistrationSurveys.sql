CREATE TABLE [dbo].[RegistrationSurveys]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RegistrationParticipationId] INT NOT NULL, 
    [CeremonyId] INT NOT NULL, 
    [Completed] DATETIME NOT NULL DEFAULT getdate(), 
    [SurveyId] INT NOT NULL, 
    CONSTRAINT [FK_SurveyRegistration_RegistrationParticipations] FOREIGN KEY ([RegistrationParticipationId]) REFERENCES [RegistrationParticipations]([Id]), 
    CONSTRAINT [FK_SurveyRegistration_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [Ceremonies]([Id]), 
    CONSTRAINT [FK_SurveyRegistration_Surveys] FOREIGN KEY ([SurveyId]) REFERENCES [Surveys]([Id]),
)

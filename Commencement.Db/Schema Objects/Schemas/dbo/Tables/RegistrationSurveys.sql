CREATE TABLE [dbo].[RegistrationSurveys] (
    [Id]                          INT      IDENTITY (1, 1) NOT NULL,
    [RegistrationParticipationId] INT      NULL,
    [CeremonyId]                  INT      NOT NULL,
    [Completed]                   DATETIME DEFAULT (getdate()) NOT NULL,
    [SurveyId]                    INT      NOT NULL,
    [RegistrationPetitionId]      INT      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RegistrationSurveys_RegistrationPetitions] FOREIGN KEY ([RegistrationPetitionId]) REFERENCES [dbo].[RegistrationPetitions] ([id]),
    CONSTRAINT [FK_SurveyRegistration_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]),
    CONSTRAINT [FK_SurveyRegistration_RegistrationParticipations] FOREIGN KEY ([RegistrationParticipationId]) REFERENCES [dbo].[RegistrationParticipations] ([id]),
    CONSTRAINT [FK_SurveyRegistration_Surveys] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Surveys] ([Id])
);





CREATE TABLE [dbo].[SurveyAnswers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[RegistrationSurveyId]      INT              NOT NULL,
    [SurveyFieldId] INT              NOT NULL,
    [Answer]      VARCHAR (MAX)    NOT NULL, 
    CONSTRAINT [FK_SurveyAnswers_SurveyRegistrations] FOREIGN KEY ([RegistrationSurveyId]) REFERENCES [RegistrationSurveys]([Id]), 
    CONSTRAINT [FK_SurveyAnswers_SurveyFields] FOREIGN KEY ([SurveyFieldId]) REFERENCES [SurveyFields]([Id]),
)

CREATE TABLE [dbo].[SurveyAnswers] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [RegistrationSurveyId] INT           NOT NULL,
    [SurveyFieldId]        INT           NOT NULL,
    [Answer]               VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyAnswers_SurveyFields] FOREIGN KEY ([SurveyFieldId]) REFERENCES [dbo].[SurveyFields] ([Id]),
    CONSTRAINT [FK_SurveyAnswers_SurveyRegistrations] FOREIGN KEY ([RegistrationSurveyId]) REFERENCES [dbo].[RegistrationSurveys] ([Id])
);



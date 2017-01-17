CREATE TABLE [dbo].[CeremonySurveys] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [CeremonyId] INT           NOT NULL,
    [CollegeId]  CHAR (2)      NOT NULL,
    [SurveyUrl]  VARCHAR (MAX) NULL,
    [SurveyId]   INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_CeremonySurveys_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]),
    CONSTRAINT [FK_CeremonySurveys_Colleges] FOREIGN KEY ([CollegeId]) REFERENCES [dbo].[Colleges] ([id]),
    CONSTRAINT [FK_CeremonySurveys_Surveys] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Surveys] ([Id])
);



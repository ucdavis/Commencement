CREATE TABLE [dbo].[CeremonySurveys]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CeremonyId] INT NOT NULL, 
    [CollegeId] CHAR(2) NOT NULL, 
    [SurveyUrl] VARCHAR(MAX) NULL, 
    [SurveyId] INT NULL, 
    CONSTRAINT [FK_CeremonySurveys_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [Ceremonies]([id]), 
    CONSTRAINT [FK_CeremonySurveys_Colleges] FOREIGN KEY ([CollegeId]) REFERENCES [Colleges]([id]), 
    CONSTRAINT [FK_CeremonySurveys_Surveys] FOREIGN KEY ([SurveyId]) REFERENCES [Surveys]([Id]) 
)

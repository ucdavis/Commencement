CREATE TABLE [dbo].[SurveyFieldOptions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(100) NOT NULL, 
    [SurveyFieldId] INT NOT NULL, 
    CONSTRAINT [FK_SurveyFieldOptions_SurveyFields] FOREIGN KEY ([SurveyFieldId]) REFERENCES [SurveyFields]([Id]),

)

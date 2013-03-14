CREATE TABLE [dbo].[SurveyFieldOptions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [SurveyFieldId] INT NOT NULL, 
    CONSTRAINT [FK_SurveyFieldOptions_SurveyFields] FOREIGN KEY ([SurveyFieldId]) REFERENCES [SurveyFields]([Id]),

)

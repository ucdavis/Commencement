CREATE TABLE [dbo].[SurveyFields]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [SurveyId]          INT          NOT NULL,
    [Prompt]            VARCHAR (50) NOT NULL,
    [SurveyFieldTypeId] INT          NOT NULL,
    [Order]           INT          CONSTRAINT [DF_FormFields_Order] DEFAULT ((0)) NOT NULL,
    [Hidden]          BIT          CONSTRAINT [DF_FormFields_Hidden] DEFAULT ((0)) NOT NULL,
    [ShowInTable]     BIT          CONSTRAINT [DF_FormFields_ShowInTable] DEFAULT ((0)) NOT NULL,
    [Export] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_SurveyFields_Surveys] FOREIGN KEY ([SurveyId]) REFERENCES [SurveyFields]([Id]), 
    CONSTRAINT [FK_SurveyFields_SurveyFieldTypes] FOREIGN KEY ([SurveyFieldTypeId]) REFERENCES [SurveyFieldTypes]([Id]), 
)

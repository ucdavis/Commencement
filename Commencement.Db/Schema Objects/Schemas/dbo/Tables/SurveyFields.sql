CREATE TABLE [dbo].[SurveyFields] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [SurveyId]          INT           NOT NULL,
    [Prompt]            VARCHAR (MAX) NOT NULL,
    [SurveyFieldTypeId] INT           NOT NULL,
    [Order]             INT           CONSTRAINT [DF_FormFields_Order] DEFAULT ((0)) NOT NULL,
    [Hidden]            BIT           CONSTRAINT [DF_FormFields_Hidden] DEFAULT ((0)) NOT NULL,
    [ShowInTable]       BIT           CONSTRAINT [DF_FormFields_ShowInTable] DEFAULT ((0)) NOT NULL,
    [Export]            BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyFields_SurveyFieldTypes] FOREIGN KEY ([SurveyFieldTypeId]) REFERENCES [dbo].[SurveyFieldTypes] ([Id]),
    CONSTRAINT [FK_SurveyFields_Surveys] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Surveys] ([Id])
);





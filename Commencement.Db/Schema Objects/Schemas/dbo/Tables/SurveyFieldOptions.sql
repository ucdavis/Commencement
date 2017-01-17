CREATE TABLE [dbo].[SurveyFieldOptions] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (MAX) NOT NULL,
    [SurveyFieldId] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SurveyFieldOptions_SurveyFields] FOREIGN KEY ([SurveyFieldId]) REFERENCES [dbo].[SurveyFields] ([Id])
);



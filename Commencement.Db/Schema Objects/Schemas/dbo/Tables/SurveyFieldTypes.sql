CREATE TABLE [dbo].[SurveyFieldTypes] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50) NOT NULL,
    [HasOptions]     BIT          CONSTRAINT [DF_FormFieldTypes_HasOptions] DEFAULT ((0)) NOT NULL,
    [Filterable]     BIT          NOT NULL,
    [Answerable]     BIT          DEFAULT ((1)) NOT NULL,
    [FixedAnswers]   BIT          DEFAULT ((0)) NOT NULL,
    [HasMultiAnswer] BIT          DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);





CREATE TABLE [dbo].[SurveyFieldTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name]       VARCHAR (50) NOT NULL,
    [HasOptions] BIT          CONSTRAINT [DF_FormFieldTypes_HasOptions] DEFAULT ((0)) NOT NULL,
    [Filterable] BIT          NOT NULL, 
    [Answerable] BIT NOT NULL DEFAULT ((1)), 
    [FixedAnswers] BIT NOT NULL DEFAULT ((0)), 
    [HasMultiAnswer] BIT NOT NULL DEFAULT 1,
)

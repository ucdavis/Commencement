CREATE TABLE [dbo].[SurveyFieldValidators] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)  NOT NULL,
    [Class]        VARCHAR (50)  NULL,
    [RegEx]        VARCHAR (MAX) NULL,
    [ErrorMessage] VARCHAR (200) NULL,
    CONSTRAINT [PK_Validators] PRIMARY KEY CLUSTERED ([id] ASC)
);


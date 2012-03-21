CREATE TABLE [dbo].[TemplateTypes] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [Code]        CHAR (2)      NULL
);


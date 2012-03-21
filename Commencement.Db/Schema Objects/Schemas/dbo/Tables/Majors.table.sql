CREATE TABLE [dbo].[Majors] (
    [id]                VARCHAR (4)   NOT NULL,
    [Name]              VARCHAR (30)  NOT NULL,
    [DisciplineCode]    VARCHAR (5)   NULL,
    [CollegeCode]       CHAR (2)      NULL,
    [ConsolidationCode] VARCHAR (4)   NULL,
    [IsActive]          BIT           NOT NULL,
    [FullName]          VARCHAR (100) NULL
);


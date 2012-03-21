CREATE TABLE [dbo].[MajorCodes] (
    [id]              VARCHAR (4)  NOT NULL,
    [Name]            VARCHAR (30) NOT NULL,
    [IsMajor]         BIT          NOT NULL,
    [IsMinor]         BIT          NOT NULL,
    [IsConcentration] BIT          NOT NULL,
    [Dept]            VARCHAR (4)  NULL,
    [DisciplineCode]  VARCHAR (5)  NULL,
    [CollegeCode]     CHAR (2)     NULL,
    [LevelCode]       CHAR (2)     NULL
);


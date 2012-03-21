CREATE TABLE [dbo].[GradeCodes] (
    [id]            VARCHAR (6)    NOT NULL,
    [Name]          VARCHAR (50)   NOT NULL,
    [QualityPoints] DECIMAL (6, 3) NOT NULL,
    [IsAttempted]   BIT            NOT NULL,
    [IsCompleted]   BIT            NOT NULL,
    [IsGPA]         BIT            NOT NULL,
    [IsPassed]      BIT            NOT NULL
);


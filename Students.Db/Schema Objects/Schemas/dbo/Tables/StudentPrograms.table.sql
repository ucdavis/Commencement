CREATE TABLE [dbo].[StudentPrograms] (
    [PIDM]         VARCHAR (8)  NOT NULL,
    [Termcode]     VARCHAR (6)  NOT NULL,
    [MajorCode]    VARCHAR (4)  NOT NULL,
    [LevelCode]    CHAR (2)     NULL,
    [DegreeCode]   VARCHAR (6)  NULL,
    [FieldOfStudy] VARCHAR (20) NULL,
    [AdmitTerm]    VARCHAR (6)  NULL,
    [AdmitCode]    VARCHAR (2)  NULL,
    [CampusCode]   VARCHAR (3)  NULL,
    [Primary]      BIT          NULL
);


CREATE TABLE [dbo].[HonorsCutoffs] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [SourceTerm]       INT            NOT NULL,
    [College]          CHAR (2)       NOT NULL,
    [MinUnits]         INT            NOT NULL,
    [HonorsGpa]        DECIMAL (4, 3) NOT NULL,
    [HighHonorsGpa]    DECIMAL (4, 3) NULL,
    [HighestHonorsGpa] DECIMAL (4, 3) NULL,
    [StartTerm]        AS             ([dbo].[udf_CalculateStartTerm]([SourceTerm])),
    [EndTerm]          AS             ([dbo].[udf_CalculateEndTerm]([SourceTerm])),
    PRIMARY KEY CLUSTERED ([Id] ASC)
);





CREATE TABLE [dbo].[HonorsCutoffs]
(
	[Id] INT NOT NULL Identity PRIMARY KEY, 
    [SourceTerm] INT NOT NULL, 
    [College] CHAR(2) NOT NULL,
	[MinUnits] int not null,
	[HonorsGpa] decimal(4,3) not null,
	[HighHonorsGpa] decimal(4,3) null,
	[HighestHonorsGpa] decimal(4,3) null, 
    [StartTerm] as dbo.udf_CalculateStartTerm(SourceTerm),
    [EndTerm] as dbo.udf_CalculateEndTerm(SourceTerm)
)

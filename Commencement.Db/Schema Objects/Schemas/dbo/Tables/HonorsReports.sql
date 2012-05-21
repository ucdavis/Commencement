﻿CREATE TABLE [dbo].[HonorsReports]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Contents] VARBINARY(MAX) NOT NULL, 
    [DateRequested] DATETIME NOT NULL DEFAULT getdate(), 
    [UserId] INT NOT NULL, 
    [TermCode] VARCHAR(6) NOT NULL, 
    [Honors4590] DECIMAL(18, 3) NOT NULL, 
    [HighHonors4590] DECIMAL(18, 3) NULL, 
    [HighestHonors4590] DECIMAL(18, 3) NULL, 
    [Honors90135] DECIMAL(18, 3) NOT NULL,
	[HighHonors90135] DECIMAL(18, 3) NULL, 
    [HighestHonors90135] DECIMAL(18, 3) NULL, 
    [Honors135] DECIMAL(18, 3) NOT NULL,
	[HighHonors135] DECIMAL(18, 3) NULL, 
    [HighestHonors135] DECIMAL(18, 3) NULL, 
    [CollegeCode] CHAR(2) NOT NULL, 
)

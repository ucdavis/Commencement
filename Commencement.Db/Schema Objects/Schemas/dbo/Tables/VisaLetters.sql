﻿CREATE TABLE [dbo].[VisaLetters]
(
	[Id] INT              IDENTITY (1, 1) NOT NULL,
	[Student_Id] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATETIME2 NOT NULL, 
    [Gender] CHAR(1) NOT NULL, 
    [Ceremony] CHAR NULL, 
    [RelativeTitle] VARCHAR(5) NOT NULL, 
    [RelativeFirstName] VARCHAR(100) NOT NULL, 
    [RelativeLastName] VARCHAR(100) NOT NULL, 
    [RelationshipToStudent] VARCHAR(100) NOT NULL, 
    [RelativeMailingAddress] VARCHAR(500) NOT NULL, 
    [IsApproved] BIT NOT NULL DEFAULT ((0)), 
    [IsPending] BIT NOT NULL DEFAULT ((1)), 
    [DateDecided] DATETIME NULL, 
    [CollegeName] VARCHAR(250) NULL, 
    [MajorName] VARCHAR(250) NULL, 
    [CeremonyDateTime] DATETIME NULL, 
    [ApprovedBy] VARCHAR(50) NULL, 
    [StudentFirstName] VARCHAR(50) NULL, 
    [StudentLastName] VARCHAR(50) NULL, 
    CONSTRAINT [PK_VisaLetters] PRIMARY KEY ([Id]), 
    
)

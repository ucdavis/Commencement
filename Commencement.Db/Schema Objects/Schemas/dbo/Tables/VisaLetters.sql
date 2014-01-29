CREATE TABLE [dbo].[VisaLetters]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Student_Id] UNIQUEIDENTIFIER NOT NULL, 
	[DateCreated] DATETIME NOT NULL,
    [Gender] CHAR NOT NULL, 
    [Ceremony] CHAR NULL, 
    [RelativeFirstName] VARCHAR(100) NOT NULL, 
    [RelativeLastName] VARCHAR(100) NOT NULL, 
    [RelationshipToStudent] VARCHAR(100) NOT NULL, 
    [RelativeMailingAddress] VARCHAR(500) NOT NULL, 
    [IsApproved] BIT NOT NULL DEFAULT 0, 
    [DateApproved] DATETIME NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1    
)

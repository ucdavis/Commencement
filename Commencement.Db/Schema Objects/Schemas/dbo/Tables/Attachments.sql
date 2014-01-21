CREATE TABLE [dbo].[Attachments]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Contents] VARBINARY(MAX) NOT NULL, 
    [ContentType] VARCHAR(50) NOT NULL, 
    [FileName] VARCHAR(250) NULL, 
    [PublicGuid] UNIQUEIDENTIFIER NULL
)

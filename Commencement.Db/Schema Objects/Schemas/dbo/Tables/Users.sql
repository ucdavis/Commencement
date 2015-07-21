CREATE TABLE [dbo].[Users]
(
	[Id] VARCHAR(10) NOT NULL PRIMARY KEY, 
    [Email] VARCHAR(50) NOT NULL, 
    [UserEmail] VARCHAR(50) NULL, 
    [Phone] VARCHAR(50) NULL, 
    [FirstName] VARCHAR(50) NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [CatbertUserId] INT NULL
)

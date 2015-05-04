CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LoginID] VARCHAR(10) NOT NULL, 
    [Email] VARCHAR(50) NOT NULL, 
    [UserEmail] VARCHAR(50) NULL, 
    [Phone] VARCHAR(50) NULL, 
    [FirstName] VARCHAR(50) NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [CatbertUserId] INT NULL
)

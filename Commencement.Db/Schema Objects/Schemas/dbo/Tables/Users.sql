CREATE TABLE [dbo].[Users] (
    [Id]            VARCHAR (10) NOT NULL,
    [Email]         VARCHAR (50) NOT NULL,
    [UserEmail]     VARCHAR (50) NULL,
    [Phone]         VARCHAR (50) NULL,
    [FirstName]     VARCHAR (50) NULL,
    [LastName]      VARCHAR (50) NOT NULL,
    [IsActive]      BIT          DEFAULT ((1)) NOT NULL,
    [CatbertUserId] INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);




CREATE TABLE [dbo].[Attachments] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Contents]    VARBINARY (MAX)  NOT NULL,
    [ContentType] VARCHAR (50)     NOT NULL,
    [FileName]    VARCHAR (250)    NULL,
    [PublicGuid]  UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);





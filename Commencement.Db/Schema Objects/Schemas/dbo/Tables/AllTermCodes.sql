CREATE TABLE [dbo].[AllTermCodes]
(
	    [id]          VARCHAR (6)  NOT NULL,
    [Description] VARCHAR (30) NULL,
    [StartDate]   DATETIME     NULL,
    [EndDate]     DATETIME     NULL,
    [TypeCode]    VARCHAR (1)  NULL,
    CONSTRAINT [PK_AllTermCodes] PRIMARY KEY CLUSTERED ([id] ASC)
)

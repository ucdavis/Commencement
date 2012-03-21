CREATE TABLE [dbo].[PageTracking] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [LoginId]     VARCHAR (50)  NOT NULL,
    [Location]    VARCHAR (500) NOT NULL,
    [IPAddress]   VARCHAR (20)  NOT NULL,
    [DateTime]    DATETIME      NOT NULL,
    [IsEmulating] BIT           NOT NULL
);


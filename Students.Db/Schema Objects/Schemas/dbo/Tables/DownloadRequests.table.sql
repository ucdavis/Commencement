CREATE TABLE [dbo].[DownloadRequests] (
    [id]            INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [StudentId]     VARCHAR (9)  NOT NULL,
    [DateRequest]   DATETIME     NOT NULL,
    [UserId]        VARCHAR (50) NOT NULL,
    [RequestedFrom] VARCHAR (50) NOT NULL,
    [Completed]     BIT          NOT NULL,
    [DateCompleted] DATETIME     NULL
);


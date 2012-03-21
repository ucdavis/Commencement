CREATE TABLE [dbo].[CeremonyEditors] (
    [id]         INT IDENTITY (1, 1) NOT NULL,
    [CeremonyId] INT NOT NULL,
    [UserId]     INT NOT NULL,
    [Owner]      BIT NOT NULL
);


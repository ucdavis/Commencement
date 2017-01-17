CREATE TABLE [dbo].[HonorsReports] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Contents]           VARBINARY (MAX) NULL,
    [DateRequested]      DATETIME        DEFAULT (getdate()) NOT NULL,
    [UserId]             INT             NOT NULL,
    [TermCode]           VARCHAR (6)     NOT NULL,
    [Honors4590]         DECIMAL (18, 3) NOT NULL,
    [HighHonors4590]     DECIMAL (18, 3) NULL,
    [HighestHonors4590]  DECIMAL (18, 3) NULL,
    [Honors90135]        DECIMAL (18, 3) NOT NULL,
    [HighHonors90135]    DECIMAL (18, 3) NULL,
    [HighestHonors90135] DECIMAL (18, 3) NULL,
    [Honors135]          DECIMAL (18, 3) NOT NULL,
    [HighHonors135]      DECIMAL (18, 3) NULL,
    [HighestHonors135]   DECIMAL (18, 3) NULL,
    [CollegeCode]        CHAR (2)        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_HonorsReports_College] FOREIGN KEY ([CollegeCode]) REFERENCES [dbo].[Colleges] ([id])
);



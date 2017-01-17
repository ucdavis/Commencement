CREATE TABLE [dbo].[VisaLetters] (
    [Id]                     INT              IDENTITY (1, 1) NOT NULL,
    [Student_Id]             UNIQUEIDENTIFIER NOT NULL,
    [DateCreated]            DATETIME2 (7)    NOT NULL,
    [Gender]                 CHAR (1)         NOT NULL,
    [Ceremony]               CHAR (1)         NULL,
    [RelativeTitle]          VARCHAR (5)      NOT NULL,
    [RelativeFirstName]      VARCHAR (100)    NOT NULL,
    [RelativeLastName]       VARCHAR (100)    NOT NULL,
    [RelationshipToStudent]  VARCHAR (100)    NOT NULL,
    [RelativeMailingAddress] VARCHAR (500)    NOT NULL,
    [IsApproved]             BIT              DEFAULT ((0)) NOT NULL,
    [IsPending]              BIT              DEFAULT ((1)) NOT NULL,
    [DateDecided]            DATETIME         NULL,
    [CollegeCode]            VARCHAR (5)      NOT NULL,
    [CollegeName]            VARCHAR (250)    NULL,
    [MajorName]              VARCHAR (250)    NULL,
    [CeremonyDateTime]       DATETIME         NULL,
    [ApprovedBy]             VARCHAR (50)     NULL,
    [StudentFirstName]       VARCHAR (50)     NULL,
    [StudentLastName]        VARCHAR (50)     NULL,
    [IsDenied]               BIT              DEFAULT ((0)) NOT NULL,
    [IsCanceled]             BIT              DEFAULT ((0)) NOT NULL,
    [LastUpdateDateTime]     DATETIME         NULL,
    [Degree]                 VARCHAR (50)     NOT NULL,
    [ReferenceGuid]          UNIQUEIDENTIFIER NULL,
    [HardCopy]               VARCHAR (1)      DEFAULT ('N') NOT NULL,
    CONSTRAINT [PK_VisaLetters] PRIMARY KEY CLUSTERED ([Id] ASC)
);





CREATE TABLE [dbo].[TermCodes] (
    [id]                     VARCHAR (6)   NOT NULL,
    [Name]                   VARCHAR (50)  NOT NULL,
    [IsActive]               BIT           CONSTRAINT [DF_TermCodes_IsActive] DEFAULT ((0)) NOT NULL,
    [LandingText]            VARCHAR (MAX) NULL,
    [RegistrationWelcome]    VARCHAR (MAX) NULL,
    [CapAndGownDeadline]     DATE          NOT NULL,
    [FileToGraduateDeadline] DATE          NOT NULL,
    [RegistrationBegin]      DATE          CONSTRAINT [DF_TermCodes_RegistrationBegin] DEFAULT (getdate()) NOT NULL,
    [RegistrationDeadline]   DATE          CONSTRAINT [DF_TermCodes_RegistrationEnd] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TermCodes] PRIMARY KEY CLUSTERED ([id] ASC)
);




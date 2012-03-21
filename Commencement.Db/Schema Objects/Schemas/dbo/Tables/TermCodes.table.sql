CREATE TABLE [dbo].[TermCodes] (
    [id]                     VARCHAR (6)   NOT NULL,
    [Name]                   VARCHAR (50)  NOT NULL,
    [IsActive]               BIT           NOT NULL,
    [LandingText]            VARCHAR (MAX) NULL,
    [RegistrationWelcome]    VARCHAR (MAX) NULL,
    [CapAndGownDeadline]     DATE          NOT NULL,
    [FileToGraduateDeadline] DATE          NOT NULL
);


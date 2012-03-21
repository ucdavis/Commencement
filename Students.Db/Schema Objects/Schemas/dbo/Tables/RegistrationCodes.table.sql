CREATE TABLE [dbo].[RegistrationCodes] (
    [id]          CHAR (2)     NOT NULL,
    [Name]        VARCHAR (50) NULL,
    [IsEnrolled]  BIT          NULL,
    [IsGraded]    BIT          NULL,
    [IsWaitlist]  BIT          NULL,
    [IsWithdrawl] BIT          NULL
);


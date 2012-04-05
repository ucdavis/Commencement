CREATE TABLE [dbo].[RegistrationPetitions] (
    [id]                INT            IDENTITY (1, 1) NOT NULL,
    [RegistrationId]    INT            NOT NULL,
    [MajorCode]         VARCHAR (4)    NOT NULL,
    [ExceptionReason]   VARCHAR (500) NOT NULL,
    [CompletionTerm]    VARCHAR (6)    NOT NULL,
    [TransferUnitsFrom] VARCHAR (100)  NULL,
    [TransferUnits]     VARCHAR (5)    NULL,
    [IsPending]         BIT            NOT NULL,
    [IsApproved]        BIT            NOT NULL,
    [DateSubmitted]     DATETIME       NOT NULL,
    [DateDecision]      DATETIME       NULL,
    [CeremonyId]        INT            NOT NULL,
    [NumberTickets]     INT            NOT NULL
);


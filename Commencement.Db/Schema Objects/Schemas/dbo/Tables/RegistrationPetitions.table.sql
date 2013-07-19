CREATE TABLE [dbo].[RegistrationPetitions] (
    [id]                         INT           IDENTITY (1, 1) NOT NULL,
    [RegistrationId]             INT           NOT NULL,
    [MajorCode]                  VARCHAR (4)   NOT NULL,
    [ExceptionReason]            VARCHAR (1000) NOT NULL,
    [CompletionTerm]             VARCHAR (6)   NULL,
    [TransferUnitsFrom]          VARCHAR (100) NULL,
    [TransferUnits]              VARCHAR (5)   NULL,
    [IsPending]                  BIT           NOT NULL,
    [IsApproved]                 BIT           NOT NULL,
    [DateSubmitted]              DATETIME      NOT NULL,
    [DateDecision]               DATETIME      NULL,
    [CeremonyId]                 INT           NOT NULL,
    [NumberTickets]              INT           NOT NULL,
    [TicketDistributionMethodId] VARCHAR (2)   NULL,
	[ExitSurvey] BIT NOT NULL DEFAULT 0
);




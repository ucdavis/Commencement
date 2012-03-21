CREATE TABLE [dbo].[RegistrationParticipations] (
    [id]                    INT         IDENTITY (1, 1) NOT NULL,
    [RegistrationId]        INT         NOT NULL,
    [MajorCode]             VARCHAR (4) NOT NULL,
    [CeremonyId]            INT         NOT NULL,
    [NumberTickets]         INT         NULL,
    [Cancelled]             BIT         NULL,
    [LabelPrinted]          BIT         NULL,
    [ExtraTicketPetitionId] INT         NULL,
    [DateRegistered]        DATETIME    NOT NULL,
    [DateUpdated]           DATETIME    NULL
);


CREATE TABLE [dbo].[ExtraTicketPetitions] (
    [id]                              INT           IDENTITY (1, 1) NOT NULL,
    [NumberTicketsRequested]          INT           NOT NULL,
    [NumberTicketsRequestedStreaming] INT           NOT NULL,
    [IsPending]                       BIT           NOT NULL,
    [IsApproved]                      BIT           NOT NULL,
    [DateSubmitted]                   DATETIME      NOT NULL,
    [DateDecision]                    DATETIME      NULL,
    [LabelPrinted]                    BIT           NOT NULL,
    [NumberTickets]                   INT           NULL,
    [NumberTicketsStreaming]          INT           NULL,
    [Reason]                          VARCHAR (100) NULL
);


﻿CREATE TABLE [dbo].[Ceremonies] (
    [id]                    INT           IDENTITY (1, 1) NOT NULL,
    [Name]                  VARCHAR (100) NULL,
    [Location]              VARCHAR (200) NOT NULL,
    [DateTime]              DATETIME      NOT NULL,
    [TicketsPerStudent]     INT           NOT NULL,
    [TotalTickets]          INT           NOT NULL,
    [TotalStreamingTickets] INT           NULL,
    [PrintingDeadline]      DATE          NOT NULL,
    [TermCode]              VARCHAR (6)   NOT NULL,
    [ExtraTicketDeadline]   DATE          NOT NULL,
    [ExtraTicketPerStudent] INT           NOT NULL,
    [MinUnits]              INT           NOT NULL,
    [PetitionThreshold]     INT           NOT NULL,
    [ExtraTicketBegin]      DATE          NOT NULL,
    [HasStreamingTickets]   BIT           NOT NULL,
    [ConfirmationText]      VARCHAR (MAX) NULL,
    [PickupTickets]         BIT           NOT NULL,
    [MailTickets]           BIT           NOT NULL,
    [WebsiteUrl]            VARCHAR (MAX) NULL,
    [SurveyUrl]             VARCHAR (MAX) NULL, 
    [SurveyId] INT NULL, 
    CONSTRAINT [FK_Ceremonies_Surveys] FOREIGN KEY ([SurveyId]) REFERENCES [Surveys]([Id])
);






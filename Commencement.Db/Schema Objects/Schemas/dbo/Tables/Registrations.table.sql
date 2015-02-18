CREATE TABLE [dbo].[Registrations] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [Student_Id]  UNIQUEIDENTIFIER NOT NULL,
    [Address1]    VARCHAR (200)    NOT NULL,
    [Address2]    VARCHAR (200)    NULL,
    [City]        VARCHAR (100)    NOT NULL,
    [State]       CHAR (2)         NOT NULL,
    [Zip]         VARCHAR (15)     NOT NULL,
    [Email]       VARCHAR (100)    NULL,
    [MailTickets] BIT              NOT NULL,
    [TermCode]    VARCHAR (6)      NOT NULL,
    [GradTrack]   BIT              NOT NULL, 
    [TicketPassword] VARCHAR(50) NULL, 
    [Phonetic] VARCHAR(150) NULL, 
    [CellNumberForText] VARCHAR(10) NULL, 
    [CellCarrier] VARCHAR(25) NULL
);


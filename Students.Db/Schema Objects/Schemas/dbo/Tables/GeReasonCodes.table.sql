CREATE TABLE [dbo].[GeReasonCodes] (
    [id]                 VARCHAR (6)  NOT NULL,
    [Name]               VARCHAR (30) NOT NULL,
    [CalculatorOverride] BIT          NULL,
    [Conditional]        BIT          NULL,
    [Graduation]         BIT          NULL,
    [Withdrawal]         BIT          NULL,
    [TypeIndicator]      VARCHAR (2)  NULL
);


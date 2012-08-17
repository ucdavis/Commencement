CREATE TABLE [dbo].[SpecialNeeds] (
    [id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsActive] BIT           NOT NULL, 
    [Tip] VARCHAR(MAX) NULL
);


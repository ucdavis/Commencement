CREATE TABLE [dbo].[Templates] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [BodyText]       VARCHAR (MAX) NOT NULL,
    [TemplateTypeId] INT           NOT NULL,
    [CeremonyId]     INT           NOT NULL,
    [IsActive]       BIT           NOT NULL,
    [Subject]        VARCHAR (100) NULL
);


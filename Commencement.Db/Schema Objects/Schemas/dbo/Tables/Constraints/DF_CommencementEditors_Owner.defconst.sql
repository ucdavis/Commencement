ALTER TABLE [dbo].[CeremonyEditors]
    ADD CONSTRAINT [DF_CommencementEditors_Owner] DEFAULT ((0)) FOR [Owner];


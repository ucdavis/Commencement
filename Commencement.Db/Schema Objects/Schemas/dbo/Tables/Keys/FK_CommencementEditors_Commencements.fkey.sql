ALTER TABLE [dbo].[CeremonyEditors]
    ADD CONSTRAINT [FK_CommencementEditors_Commencements] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


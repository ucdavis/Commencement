ALTER TABLE [dbo].[Templates]
    ADD CONSTRAINT [FK_Templates_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


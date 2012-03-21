ALTER TABLE [dbo].[Students]
    ADD CONSTRAINT [FK_Students_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


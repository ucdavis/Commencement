ALTER TABLE [dbo].[CeremonyColleges]
    ADD CONSTRAINT [FK_CeremonyColleges_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


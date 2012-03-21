ALTER TABLE [dbo].[CeremonyMajors]
    ADD CONSTRAINT [FK_CommencementMajors_Commencements] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


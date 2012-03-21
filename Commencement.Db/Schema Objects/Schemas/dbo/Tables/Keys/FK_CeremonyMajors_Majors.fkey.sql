ALTER TABLE [dbo].[CeremonyMajors]
    ADD CONSTRAINT [FK_CeremonyMajors_Majors] FOREIGN KEY ([MajorCode]) REFERENCES [dbo].[Majors] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


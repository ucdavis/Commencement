ALTER TABLE [dbo].[CeremonyColleges]
    ADD CONSTRAINT [FK_CeremonyColleges_Colleges] FOREIGN KEY ([CollegeCode]) REFERENCES [dbo].[Colleges] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


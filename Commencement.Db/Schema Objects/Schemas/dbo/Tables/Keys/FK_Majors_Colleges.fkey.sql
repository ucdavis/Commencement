ALTER TABLE [dbo].[Majors]
    ADD CONSTRAINT [FK_Majors_Colleges] FOREIGN KEY ([CollegeCode]) REFERENCES [dbo].[Colleges] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


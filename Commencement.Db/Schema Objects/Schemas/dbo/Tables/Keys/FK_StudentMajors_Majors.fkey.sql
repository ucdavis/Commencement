ALTER TABLE [dbo].[StudentMajors]
    ADD CONSTRAINT [FK_StudentMajors_Majors] FOREIGN KEY ([MajorCode]) REFERENCES [dbo].[Majors] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


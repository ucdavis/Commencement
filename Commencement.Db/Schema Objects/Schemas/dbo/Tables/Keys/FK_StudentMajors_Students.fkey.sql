ALTER TABLE [dbo].[StudentMajors]
    ADD CONSTRAINT [FK_StudentMajors_Students] FOREIGN KEY ([Student_Id]) REFERENCES [dbo].[Students] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


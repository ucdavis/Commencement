ALTER TABLE [dbo].[EmailQueue]
    ADD CONSTRAINT [FK_EmailQueue_Students] FOREIGN KEY ([Student_Id]) REFERENCES [dbo].[Students] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


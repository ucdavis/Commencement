ALTER TABLE [dbo].[EmailQueue]
    ADD CONSTRAINT [FK_EmailQueue_Templates] FOREIGN KEY ([TemplateId]) REFERENCES [dbo].[Templates] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


ALTER TABLE [dbo].[Templates]
    ADD CONSTRAINT [FK_Templates_TemplateTypes] FOREIGN KEY ([TemplateTypeId]) REFERENCES [dbo].[TemplateTypes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


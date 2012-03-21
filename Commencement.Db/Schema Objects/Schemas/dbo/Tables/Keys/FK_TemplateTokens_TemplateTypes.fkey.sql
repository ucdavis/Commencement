ALTER TABLE [dbo].[TemplateTokens]
    ADD CONSTRAINT [FK_TemplateTokens_TemplateTypes] FOREIGN KEY ([TemplateTypeId]) REFERENCES [dbo].[TemplateTypes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


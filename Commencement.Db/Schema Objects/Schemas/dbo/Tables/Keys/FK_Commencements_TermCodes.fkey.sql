ALTER TABLE [dbo].[Ceremonies]
    ADD CONSTRAINT [FK_Commencements_TermCodes] FOREIGN KEY ([TermCode]) REFERENCES [dbo].[TermCodes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


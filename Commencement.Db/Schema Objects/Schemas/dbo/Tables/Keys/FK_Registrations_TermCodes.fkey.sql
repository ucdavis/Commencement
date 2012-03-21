ALTER TABLE [dbo].[Registrations]
    ADD CONSTRAINT [FK_Registrations_TermCodes] FOREIGN KEY ([TermCode]) REFERENCES [dbo].[TermCodes] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


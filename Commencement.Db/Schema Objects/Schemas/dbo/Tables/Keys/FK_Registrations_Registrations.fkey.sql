ALTER TABLE [dbo].[Registrations]
    ADD CONSTRAINT [FK_Registrations_Registrations] FOREIGN KEY ([id]) REFERENCES [dbo].[Registrations] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


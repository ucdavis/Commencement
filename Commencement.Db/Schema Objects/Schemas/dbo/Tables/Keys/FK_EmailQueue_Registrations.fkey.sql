ALTER TABLE [dbo].[EmailQueue]
    ADD CONSTRAINT [FK_EmailQueue_Registrations] FOREIGN KEY ([RegistrationId]) REFERENCES [dbo].[Registrations] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


ALTER TABLE [dbo].[RegistrationPetitions]
    ADD CONSTRAINT [FK_RegistrationPetitions_RegistrationPetitions] FOREIGN KEY ([RegistrationId]) REFERENCES [dbo].[Registrations] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


ALTER TABLE [dbo].[RegistrationParticipations]
    ADD CONSTRAINT [FK_RegistrationParticipations_Registrations] FOREIGN KEY ([RegistrationId]) REFERENCES [dbo].[Registrations] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


ALTER TABLE [dbo].[RegistrationSpecialNeeds]
    ADD CONSTRAINT [FK_RegistrationSpecialNeeds_Registrations] FOREIGN KEY ([RegistrationId]) REFERENCES [dbo].[Registrations] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


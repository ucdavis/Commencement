ALTER TABLE [dbo].[EmailQueue]
    ADD CONSTRAINT [FK_EmailQueue_RegistrationPetitions] FOREIGN KEY ([RegistrationPetitionId]) REFERENCES [dbo].[RegistrationPetitions] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


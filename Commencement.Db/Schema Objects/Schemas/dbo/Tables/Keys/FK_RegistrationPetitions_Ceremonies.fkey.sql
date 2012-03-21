ALTER TABLE [dbo].[RegistrationPetitions]
    ADD CONSTRAINT [FK_RegistrationPetitions_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


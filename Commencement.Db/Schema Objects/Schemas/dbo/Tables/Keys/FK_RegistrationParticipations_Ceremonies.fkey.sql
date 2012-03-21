ALTER TABLE [dbo].[RegistrationParticipations]
    ADD CONSTRAINT [FK_RegistrationParticipations_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


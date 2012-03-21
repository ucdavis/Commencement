ALTER TABLE [dbo].[RegistrationParticipations]
    ADD CONSTRAINT [FK_RegistrationParticipations_ExtraTicketPetitions] FOREIGN KEY ([ExtraTicketPetitionId]) REFERENCES [dbo].[ExtraTicketPetitions] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


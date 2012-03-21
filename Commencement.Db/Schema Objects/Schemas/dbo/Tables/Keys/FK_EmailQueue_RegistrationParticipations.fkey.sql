ALTER TABLE [dbo].[EmailQueue]
    ADD CONSTRAINT [FK_EmailQueue_RegistrationParticipations] FOREIGN KEY ([RegistrationParticipationId]) REFERENCES [dbo].[RegistrationParticipations] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


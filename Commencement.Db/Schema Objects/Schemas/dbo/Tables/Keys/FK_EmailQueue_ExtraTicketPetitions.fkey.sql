ALTER TABLE [dbo].[EmailQueue]
    ADD CONSTRAINT [FK_EmailQueue_ExtraTicketPetitions] FOREIGN KEY ([ExtraTicketPetitionId]) REFERENCES [dbo].[ExtraTicketPetitions] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


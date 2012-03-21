ALTER TABLE [dbo].[Registrations]
    ADD CONSTRAINT [FK_Registrations_States] FOREIGN KEY ([State]) REFERENCES [dbo].[States] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


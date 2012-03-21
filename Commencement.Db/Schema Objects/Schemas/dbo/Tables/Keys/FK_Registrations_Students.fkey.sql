ALTER TABLE [dbo].[Registrations]
    ADD CONSTRAINT [FK_Registrations_Students] FOREIGN KEY ([Student_Id]) REFERENCES [dbo].[Students] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


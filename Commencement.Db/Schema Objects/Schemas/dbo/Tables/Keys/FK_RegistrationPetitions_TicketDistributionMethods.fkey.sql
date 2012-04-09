ALTER TABLE [dbo].[RegistrationPetitions]
    ADD CONSTRAINT [FK_RegistrationPetitions_TicketDistributionMethods] FOREIGN KEY ([TicketDistributionMethodId]) REFERENCES [dbo].[TicketDistributionMethods] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


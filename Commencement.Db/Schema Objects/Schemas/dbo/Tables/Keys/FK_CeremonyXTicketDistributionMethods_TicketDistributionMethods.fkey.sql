ALTER TABLE [dbo].[CeremonyXTicketDistributionMethods]
    ADD CONSTRAINT [FK_CeremonyXTicketDistributionMethods_TicketDistributionMethods] FOREIGN KEY ([TicketDistributionMethodId]) REFERENCES [dbo].[TicketDistributionMethods] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


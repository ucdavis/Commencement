ALTER TABLE [dbo].[CeremonyXTicketDistributionMethods]
    ADD CONSTRAINT [FK_CeremonyXTicketDistributionMethods_Ceremonies] FOREIGN KEY ([CeremonyId]) REFERENCES [dbo].[Ceremonies] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;


ALTER TABLE [dbo].[CeremonyXTicketDistributionMethods]
    ADD CONSTRAINT [PK_CeremonyXTicketDistributionMethods] PRIMARY KEY CLUSTERED ([CeremonyId] ASC, [TicketDistributionMethodId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


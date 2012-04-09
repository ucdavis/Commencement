ALTER TABLE [dbo].[TicketDistributionMethods]
    ADD CONSTRAINT [DF_TicketDistributionMethods_IsActive] DEFAULT ((1)) FOR [IsActive];


ALTER TABLE [dbo].[Ceremonies]
    ADD CONSTRAINT [DF_Ceremonies_HasStreamingTickets] DEFAULT ((0)) FOR [HasStreamingTickets];


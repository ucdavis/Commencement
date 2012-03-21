ALTER TABLE [dbo].[ExtraTicketPetitions]
    ADD CONSTRAINT [DF_ExtraTicketPetitions_IsPending] DEFAULT ((1)) FOR [IsPending];


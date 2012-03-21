ALTER TABLE [dbo].[ExtraTicketPetitions]
    ADD CONSTRAINT [DF_ExtraTicketPetitions_IsApproved] DEFAULT ((0)) FOR [IsApproved];


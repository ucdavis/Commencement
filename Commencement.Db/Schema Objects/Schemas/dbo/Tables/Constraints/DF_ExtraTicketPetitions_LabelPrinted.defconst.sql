ALTER TABLE [dbo].[ExtraTicketPetitions]
    ADD CONSTRAINT [DF_ExtraTicketPetitions_LabelPrinted] DEFAULT ((0)) FOR [LabelPrinted];


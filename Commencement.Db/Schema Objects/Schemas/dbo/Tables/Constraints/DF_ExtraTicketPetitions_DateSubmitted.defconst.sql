ALTER TABLE [dbo].[ExtraTicketPetitions]
    ADD CONSTRAINT [DF_ExtraTicketPetitions_DateSubmitted] DEFAULT (getdate()) FOR [DateSubmitted];


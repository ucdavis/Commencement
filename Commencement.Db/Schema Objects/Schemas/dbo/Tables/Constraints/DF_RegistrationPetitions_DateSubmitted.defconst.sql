ALTER TABLE [dbo].[RegistrationPetitions]
    ADD CONSTRAINT [DF_RegistrationPetitions_DateSubmitted] DEFAULT (getdate()) FOR [DateSubmitted];


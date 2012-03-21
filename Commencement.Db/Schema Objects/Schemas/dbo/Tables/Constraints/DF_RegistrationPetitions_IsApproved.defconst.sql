ALTER TABLE [dbo].[RegistrationPetitions]
    ADD CONSTRAINT [DF_RegistrationPetitions_IsApproved] DEFAULT ((0)) FOR [IsApproved];


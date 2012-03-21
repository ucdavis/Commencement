ALTER TABLE [dbo].[RegistrationPetitions]
    ADD CONSTRAINT [DF_RegistrationPetitions_IsPending] DEFAULT ((1)) FOR [IsPending];


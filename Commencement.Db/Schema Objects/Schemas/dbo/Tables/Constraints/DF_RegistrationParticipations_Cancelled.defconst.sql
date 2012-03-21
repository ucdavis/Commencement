ALTER TABLE [dbo].[RegistrationParticipations]
    ADD CONSTRAINT [DF_RegistrationParticipations_Cancelled] DEFAULT ((0)) FOR [Cancelled];


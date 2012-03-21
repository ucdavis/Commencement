ALTER TABLE [dbo].[RegistrationParticipations]
    ADD CONSTRAINT [DF_RegistrationParticipations_LabelPrinted] DEFAULT ((0)) FOR [LabelPrinted];


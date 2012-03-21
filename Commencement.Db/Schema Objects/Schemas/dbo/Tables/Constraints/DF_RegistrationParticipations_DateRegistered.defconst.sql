ALTER TABLE [dbo].[RegistrationParticipations]
    ADD CONSTRAINT [DF_RegistrationParticipations_DateRegistered] DEFAULT (getdate()) FOR [DateRegistered];


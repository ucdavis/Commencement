ALTER TABLE [dbo].[Registrations]
    ADD CONSTRAINT [DF_Registrations_GradTrack] DEFAULT ((0)) FOR [GradTrack];


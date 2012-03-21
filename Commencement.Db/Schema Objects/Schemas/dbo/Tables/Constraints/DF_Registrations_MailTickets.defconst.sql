ALTER TABLE [dbo].[Registrations]
    ADD CONSTRAINT [DF_Registrations_MailTickets] DEFAULT ((0)) FOR [MailTickets];


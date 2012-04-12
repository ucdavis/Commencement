SET IDENTITY_INSERT [dbo].[TemplateTypes] ON
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description], Code)
  VALUES(1, 'Registration Confirmation', 'E-Mail sent to student after they have completed registration or modified registration', 'RC')
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description], Code)
  VALUES(2, 'Registration Petition', 'E-Mail sent to student after they have submitted a petition for registration.', 'RP')
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description], Code)
  VALUES(3, 'Ticket Petition', 'E-Mail sent to student after they have submitted a petition for extra tickets.', 'TP')
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description], Code)
  VALUES(4, 'Registration Petition - Approved', 'E-Mail sent to student once they have been granted access to register in the system.', 'RA')
GO
SET IDENTITY_INSERT [dbo].[TemplateTypes] OFF
GO
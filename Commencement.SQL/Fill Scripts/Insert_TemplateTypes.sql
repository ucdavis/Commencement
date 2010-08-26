SET IDENTITY_INSERT [dbo].[TemplateTypes] ON
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description])
  VALUES(1, 'Registration Confirmation', 'E-Mail sent to student after they have completed registration or modified registration')
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description])
  VALUES(2, 'Registration Petition', 'E-Mail sent to student after they have submitted a petition for registration.')
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description])
  VALUES(3, 'Ticket Petition', 'E-Mail sent to student after they have submitted a petition for extra tickets.')
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description])
  VALUES(4, 'Registration Petition - Approved', 'E-Mail sent to student once they have been granted access to register in the system.')
GO
INSERT INTO [dbo].[TemplateTypes]([id], [Name], [Description])
  VALUES(5, 'Ticket Petition - Decision', 'E-Mail sent to student after a decision has been made on their extra ticket petition.')
GO
SET IDENTITY_INSERT [dbo].[TemplateTypes] OFF
GO
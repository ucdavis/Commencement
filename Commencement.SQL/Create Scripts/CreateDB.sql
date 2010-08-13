USE [Commencement]
GO
/****** Object:  Table [dbo].[AuditActionTypes]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AuditActionTypes](
	[ID] [char](1) NOT NULL,
	[ActionCodeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ActionCodes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RegistrationPetitions]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RegistrationPetitions](
	[id] [int] NOT NULL,
	[StudentId] [varchar](9) NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[MajorCode] [varchar](4) NOT NULL,
	[Units] [decimal](5, 3) NULL,
	[ExceptionReason] [varchar](1000) NOT NULL,
	[CompletionTerm] [varchar](6) NOT NULL,
 CONSTRAINT [PK_RegistrationPetitions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Students]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Students](
	[Id] [uniqueidentifier] NOT NULL,
	[Pidm] [varchar](8) NOT NULL,
	[StudentId] [varchar](9) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Units] [decimal](6, 3) NULL,
	[Email] [varchar](100) NULL,
	[Login] [varchar](50) NULL,
	[DegreeCode] [char](2) NULL,
	[DateAdded] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[TermCode] [varchar](6) NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StudentMajors]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StudentMajors](
	[Student_Id] [uniqueidentifier] NOT NULL,
	[MajorCode] [varchar](4) NOT NULL,
 CONSTRAINT [PK_StudentMajors] PRIMARY KEY CLUSTERED 
(
	[Student_Id] ASC,
	[MajorCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[States]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[States](
	[Id] [char](2) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Schools]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Schools](
	[id] [char](2) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Schools] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PageTracking]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PageTracking](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[LoginId] [varchar](50) NOT NULL,
	[Location] [varchar](500) NOT NULL,
	[IPAddress] [varchar](20) NOT NULL,
	[DateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_PageTracking] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExtraTicketPetitions]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtraTicketPetitions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[NumberTickets] [int] NOT NULL,
	[IsPending] [bit] NOT NULL,
	[IsApproved] [bit] NOT NULL,
 CONSTRAINT [PK_ExtraTicketPetitions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TermCodes]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TermCodes](
	[id] [varchar](6) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TermCodes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TemplateTypes]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TemplateTypes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
 CONSTRAINT [PK_TemplateTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vTermCodes]    Script Date: 08/13/2010 12:56:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vTermCodes]
AS
SELECT     id, Description, StartDate, EndDate, TypeCode
FROM         Students.dbo.TermCodes
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TermCodes (Students.dbo)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vTermCodes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vTermCodes'
GO
/****** Object:  View [dbo].[vMajors]    Script Date: 08/13/2010 12:56:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vMajors]
AS
SELECT     id, Name, IsMajor, IsMinor, IsConcentration, Dept, DisciplineCode
FROM         Students.dbo.MajorCodes
WHERE     (IsMajor = 1)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "MajorCodes (Students.dbo)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 204
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vMajors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vMajors'
GO
/****** Object:  Table [dbo].[Templates]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Templates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[BodyText] [varchar](max) NOT NULL,
	[RegistrationConfirmation] [bit] NOT NULL,
	[RegistrationPetition] [bit] NOT NULL,
	[ExtraTicketPetition] [bit] NOT NULL,
	[TemplateTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ceremonies]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ceremonies](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Location] [varchar](200) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[TicketsPerStudent] [int] NOT NULL,
	[TotalTickets] [int] NOT NULL,
	[PrintingDeadline] [datetime] NOT NULL,
	[RegistrationDeadline] [datetime] NOT NULL,
	[TermCode] [varchar](6) NOT NULL,
 CONSTRAINT [PK_Commencements] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Audits]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Audits](
	[ID] [uniqueidentifier] NOT NULL,
	[ObjectName] [varchar](50) NOT NULL,
	[ObjectId] [varchar](50) NULL,
	[AuditActionTypeId] [char](1) NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[AuditDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Registrations]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Registrations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [uniqueidentifier] NOT NULL,
	[MajorCode] [varchar](4) NOT NULL,
	[Address1] [varchar](200) NULL,
	[Address2] [varchar](200) NULL,
	[Address3] [varchar](200) NULL,
	[City] [varchar](100) NULL,
	[State] [char](2) NULL,
	[Zip] [varchar](15) NULL,
	[Email] [varchar](100) NULL,
	[NumberTickets] [int] NOT NULL,
	[MailTickets] [bit] NOT NULL,
	[CeremonyId] [int] NOT NULL,
	[Comments] [varchar](1000) NULL,
	[ExtraTicketPetitionId] [int] NULL,
 CONSTRAINT [PK_Registrations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Petitions]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Petitions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Pidm] [varchar](8) NOT NULL,
	[Address1] [varchar](200) NULL,
	[Address2] [varchar](200) NULL,
	[Address3] [varchar](200) NULL,
	[City] [varchar](100) NULL,
	[State] [char](2) NULL,
	[Zip] [varchar](15) NULL,
	[Email] [varchar](100) NULL,
	[NumberTickets] [int] NOT NULL,
	[MailTickets] [bit] NOT NULL,
	[Pending] [bit] NOT NULL,
	[Approved] [bit] NOT NULL,
	[TermCode] [varchar](6) NOT NULL,
	[CeremonyId] [int] NULL,
 CONSTRAINT [PK_Petitions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CeremonyMajors]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CeremonyMajors](
	[CeremonyId] [int] NOT NULL,
	[MajorCode] [varchar](4) NOT NULL,
 CONSTRAINT [PK_CommencementMajors] PRIMARY KEY CLUSTERED 
(
	[CeremonyId] ASC,
	[MajorCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CeremonyEditors]    Script Date: 08/13/2010 12:56:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CeremonyEditors](
	[id] [int] NOT NULL,
	[CeremonyId] [int] NOT NULL,
	[LoginId] [varchar](50) NOT NULL,
	[Owner] [bit] NOT NULL,
 CONSTRAINT [PK_CommencementEditors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_CommencementEditors_Owner]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[CeremonyEditors] ADD  CONSTRAINT [DF_CommencementEditors_Owner]  DEFAULT ((0)) FOR [Owner]
GO
/****** Object:  Default [DF_ExtraTicketPetitions_IsPending]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[ExtraTicketPetitions] ADD  CONSTRAINT [DF_ExtraTicketPetitions_IsPending]  DEFAULT ((1)) FOR [IsPending]
GO
/****** Object:  Default [DF_ExtraTicketPetitions_IsApproved]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[ExtraTicketPetitions] ADD  CONSTRAINT [DF_ExtraTicketPetitions_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
/****** Object:  Default [DF_PageTracking_DateTime]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[PageTracking] ADD  CONSTRAINT [DF_PageTracking_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO
/****** Object:  Default [DF_Registrations_MailTickets]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Registrations] ADD  CONSTRAINT [DF_Registrations_MailTickets]  DEFAULT ((0)) FOR [MailTickets]
GO
/****** Object:  Default [DF_Students_Id]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_Id]  DEFAULT (newid()) FOR [Id]
GO
/****** Object:  Default [DF_Students_DatedAdded]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_DatedAdded]  DEFAULT (getdate()) FOR [DateAdded]
GO
/****** Object:  Default [DF_Students_DateUpdated]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_DateUpdated]  DEFAULT (getdate()) FOR [DateUpdated]
GO
/****** Object:  Default [DF_Templates_RegistrationConfirmation]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Templates] ADD  CONSTRAINT [DF_Templates_RegistrationConfirmation]  DEFAULT ((0)) FOR [RegistrationConfirmation]
GO
/****** Object:  Default [DF_Templates_RegistrationPetition]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Templates] ADD  CONSTRAINT [DF_Templates_RegistrationPetition]  DEFAULT ((0)) FOR [RegistrationPetition]
GO
/****** Object:  Default [DF_Templates_ExtraTicketPetition]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Templates] ADD  CONSTRAINT [DF_Templates_ExtraTicketPetition]  DEFAULT ((0)) FOR [ExtraTicketPetition]
GO
/****** Object:  Default [DF_TermCodes_IsActive]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[TermCodes] ADD  CONSTRAINT [DF_TermCodes_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
/****** Object:  ForeignKey [FK_Audits_ActionCodes1]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Audits]  WITH CHECK ADD  CONSTRAINT [FK_Audits_ActionCodes1] FOREIGN KEY([AuditActionTypeId])
REFERENCES [dbo].[AuditActionTypes] ([ID])
GO
ALTER TABLE [dbo].[Audits] CHECK CONSTRAINT [FK_Audits_ActionCodes1]
GO
/****** Object:  ForeignKey [FK_Commencements_TermCodes]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Ceremonies]  WITH CHECK ADD  CONSTRAINT [FK_Commencements_TermCodes] FOREIGN KEY([TermCode])
REFERENCES [dbo].[TermCodes] ([id])
GO
ALTER TABLE [dbo].[Ceremonies] CHECK CONSTRAINT [FK_Commencements_TermCodes]
GO
/****** Object:  ForeignKey [FK_CommencementEditors_Commencements]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[CeremonyEditors]  WITH CHECK ADD  CONSTRAINT [FK_CommencementEditors_Commencements] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[CeremonyEditors] CHECK CONSTRAINT [FK_CommencementEditors_Commencements]
GO
/****** Object:  ForeignKey [FK_CommencementMajors_Commencements]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[CeremonyMajors]  WITH CHECK ADD  CONSTRAINT [FK_CommencementMajors_Commencements] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[CeremonyMajors] CHECK CONSTRAINT [FK_CommencementMajors_Commencements]
GO
/****** Object:  ForeignKey [FK_Petitions_Ceremonies]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Petitions]  WITH CHECK ADD  CONSTRAINT [FK_Petitions_Ceremonies] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[Petitions] CHECK CONSTRAINT [FK_Petitions_Ceremonies]
GO
/****** Object:  ForeignKey [FK_Registrations_Commencements]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_Commencements] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_Commencements]
GO
/****** Object:  ForeignKey [FK_Registrations_ExtraTicketPetitions]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_ExtraTicketPetitions] FOREIGN KEY([ExtraTicketPetitionId])
REFERENCES [dbo].[ExtraTicketPetitions] ([id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_ExtraTicketPetitions]
GO
/****** Object:  ForeignKey [FK_Registrations_Registrations]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_Registrations] FOREIGN KEY([id])
REFERENCES [dbo].[Registrations] ([id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_Registrations]
GO
/****** Object:  ForeignKey [FK_Registrations_States]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_States] FOREIGN KEY([State])
REFERENCES [dbo].[States] ([Id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_States]
GO
/****** Object:  ForeignKey [FK_Registrations_Students]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_Students] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_Students]
GO
/****** Object:  ForeignKey [FK_Templates_TemplateTypes]    Script Date: 08/13/2010 12:56:33 ******/
ALTER TABLE [dbo].[Templates]  WITH CHECK ADD  CONSTRAINT [FK_Templates_TemplateTypes] FOREIGN KEY([TemplateTypeId])
REFERENCES [dbo].[TemplateTypes] ([id])
GO
ALTER TABLE [dbo].[Templates] CHECK CONSTRAINT [FK_Templates_TemplateTypes]
GO

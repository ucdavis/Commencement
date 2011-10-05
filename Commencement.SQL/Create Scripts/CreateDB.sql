USE [Commencement]
GO
/****** Object:  Table [dbo].[ExtraTicketPetitions]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ExtraTicketPetitions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[NumberTicketsRequested] [int] NOT NULL,
	[NumberTicketsRequestedStreaming] [int] NOT NULL,
	[IsPending] [bit] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[DateSubmitted] [datetime] NOT NULL,
	[DateDecision] [datetime] NULL,
	[LabelPrinted] [bit] NOT NULL,
	[NumberTickets] [int] NULL,
	[NumberTicketsStreaming] [int] NULL,
	[Reason] [varchar](100) NULL,
 CONSTRAINT [PK_ExtraTicketPetitions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Colleges]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Colleges](
	[id] [char](2) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Display] [bit] NOT NULL,
 CONSTRAINT [PK_Colleges] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TermCodes]    Script Date: 10/05/2011 14:30:20 ******/
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
	[LandingText] [varchar](max) NULL,
	[RegistrationWelcome] [varchar](max) NULL,
	[CapAndGownDeadline] [date] NOT NULL,
	[FileToGraduateDeadline] [date] NOT NULL,
 CONSTRAINT [PK_TermCodes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TemplateTypes]    Script Date: 10/05/2011 14:30:20 ******/
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
	[Code] [char](2) NULL,
 CONSTRAINT [PK_TemplateTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[States]    Script Date: 10/05/2011 14:30:20 ******/
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
/****** Object:  Table [dbo].[SpecialNeeds]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SpecialNeeds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SpecialNeeds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Schools]    Script Date: 10/05/2011 14:30:20 ******/
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
/****** Object:  Table [dbo].[AuditActionTypes]    Script Date: 10/05/2011 14:30:20 ******/
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
/****** Object:  Table [dbo].[PageTracking]    Script Date: 10/05/2011 14:30:20 ******/
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
	[IsEmulating] [bit] NOT NULL,
 CONSTRAINT [PK_PageTracking] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[usp_SearchStudentByLogin]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[usp_SearchStudentByLogin]
	(
		@login varchar(50)
	)

AS

	declare @tsql varchar(max)
	
	set @login = upper(@login)
	
	set @tsql = '
		select * from openquery(sis, ''
			select spriden_pidm as pidm, spriden_id as studentId
			, spriden_first_name  as firstName, spriden_mi as mi, spriden_last_name as lastName
			, email.goremal_email_address as email
			, shrlgpa_hours_earned earnedUnits, 0 as currentUnits
			, zgvlcfs_majr_code as major
			, zgvlcfs_term_code_eff as lastTerm
			, shrttrm_astd_code_end_of_term as astd
			, lower(wormoth_login_id) as loginid
		from wormoth
			inner join zgvlcfs on wormoth_pidm = zgvlcfs_pidm
			inner join spriden on wormoth_pidm = spriden_pidm
			inner join shrlgpa on wormoth_pidm = shrlgpa_pidm
			left outer join shrttrm on wormoth_pidm = shrttrm_pidm
			left outer join (
				select goremal_pidm, goremal_email_address
				from goremal
				where goremal_emal_code = ''''UCD''''
					and goremal_status_ind = ''''A''''
			) email on email.goremal_pidm = wormoth_pidm
		where wormoth_login_id = ''''' + @login + '''''
		  and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
		  and spriden_change_ind is null
		  and shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''UG''''
		  and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where ishrttrm.shrttrm_pidm = shrttrm.shrttrm_pidm and shrttrm_astd_code_end_of_term is not null )
		'')
	'

	--set @tsql = '
	--	select * from openquery(sis, ''
	--		select spriden_pidm as pidm, spriden_id as studentId
	--			, spriden_first_name as firstName, spriden_mi as mi, spriden_last_name as lastName
	--			, email.goremal_email_address as email
	--			, earnedunits.units as earnedunits
	--			, 0 as currentunits
	--			, zgvlcfs_majr_code as major
	--			, zgvlcfs_term_code_eff as lastTerm
	--			, shrttrm_astd_code_end_of_term as astd
	--			, lower(wormoth_login_id) as loginid
	--		from wormoth 
	--			inner join zgvlcfs on wormoth_pidm = zgvlcfs_pidm
	--			inner join spriden on wormoth_pidm = spriden_pidm
	--			left outer join (
	--				select shrttrm_pidm as pidm, shrttrm_astd_code_end_of_term
	--				from shrttrm
	--				where shrttrm_term_code in (select max(shrttrm_term_code) from shrttrm ishrttrm
	--											where ishrttrm.shrttrm_Pidm = shrttrm.shrttrm_pidm)
	--			) astd on astd.pidm = wormoth_pidm
	--			left outer join (
	--				select goremal_pidm, goremal_email_address
	--				from goremal
	--				where goremal_emal_code = ''''UCD''''
	--					and goremal_status_ind = ''''A''''
	--			) email on email.goremal_pidm = wormoth_pidm
	--			inner join (
	--				select shrlgpa_pidm as pidm, shrlgpa_hours_earned units
	--				from shrlgpa
	--				where shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''UG''''
	--			) EarnedUnits on EarnedUnits.pidm = zgvlcfs_pidm
	--		where wormoth_login_id = ''''' + @login + '''''
	--			and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
	--			and spriden_change_ind is null
	--	'')'

exec(@tsql)
GO
/****** Object:  StoredProcedure [dbo].[usp_SearchStudent]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[usp_SearchStudent]
	(
		@studentid varchar(9)
	)

AS

	IF object_id('tempdb..#Students') IS NOT NULL
	BEGIN
		DROP TABLE #Students
	END

	CREATE TABLE #Students
	(
		pidm varchar(8),
		studentid varchar(9),
		firstName varchar(50),
		MI varchar(50),
		LastName varchar(50),
		Email varchar(100),
		EarnedUnits decimal(6,3),
		CurrentUnits decimal(6,3),
		major varchar(4),	
		lastTerm varchar(6),
		astd varchar(2),
		LoginId varchar(50)
	)


	declare @tsql varchar(max)
	
	set @tsql = '
		insert into #Students
		select pidm, studentid
			 , firstname, mi, lastname
			 , email, earnedunits, currentunits
			 , major, lastterm, astd, loginid
			from openquery (sis, ''
			select spriden_pidm as pidm, spriden_id as studentId
			, spriden_first_name  as firstName, spriden_mi as mi, spriden_last_name as lastName
			, email.goremal_email_address as email
			, shrlgpa_hours_earned earnedUnits, 0 as currentUnits
			, zgvlcfs_majr_code as major
			, zgvlcfs_term_code_eff as lastTerm
			, shrttrm_astd_code_end_of_term as astd
			, lower(wormoth_login_id) as loginid
		from spriden
			inner join zgvlcfs on spriden_pidm = zgvlcfs_pidm
			inner join wormoth on spriden_pidm = wormoth_pidm
			inner join shrlgpa on spriden_pidm = shrlgpa_pidm
			left outer join shrttrm on spriden_pidm = shrttrm_pidm
			left outer join (
				select goremal_pidm, goremal_email_address
				from goremal
				where goremal_emal_code = ''''UCD''''
				  and goremal_status_ind = ''''A''''
			) email on email.goremal_pidm = spriden_pidm
		where spriden_id = ''''' + @studentid + '''''
		  and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
		  and spriden_change_ind is null
		  and shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''UG''''
		  and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where ishrttrm.shrttrm_pidm = shrttrm.shrttrm_pidm and shrttrm_astd_code_end_of_term is not null )
		'')
	'
	exec(@tsql)


	if not exists (select * from #Students)
	begin

		set @tsql = '
			insert into #Students
			select pidm, studentid
				 , firstname, mi, lastname
				 , email, earnedunits, currentunits
				 , major, lastterm, astd, loginid
				from openquery (sis, ''
				select spriden_pidm as pidm, spriden_id as studentId
				, spriden_first_name  as firstName, spriden_mi as mi, spriden_last_name as lastName
				, email.goremal_email_address as email
				, shrlgpa_hours_earned earnedUnits, 0 as currentUnits
				, zgvlcfs_majr_code as major
				, zgvlcfs_term_code_eff as lastTerm
				, shrttrm_astd_code_end_of_term as astd
				, lower(wormoth_login_id) as loginid
			from spriden
				inner join zgvlcfs on spriden_pidm = zgvlcfs_pidm
				inner join wormoth on spriden_pidm = wormoth_pidm
				inner join shrlgpa on spriden_pidm = shrlgpa_pidm
				left outer join shrttrm on spriden_pidm = shrttrm_pidm
				left outer join (
					select goremal_pidm, goremal_email_address
					from goremal
					where goremal_emal_code = ''''UCD''''
					  and goremal_status_ind = ''''A''''
				) email on email.goremal_pidm = spriden_pidm
			where spriden_id = ''''' + @studentid + '''''
			  and zgvlcfs_term_code_eff in (select max(zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where izgvlcfs.zgvlcfs_pidm = zgvlcfs.zgvlcfs_pidm)
			  and spriden_change_ind is null
			  and shrlgpa_gpa_type_ind = ''''O'''' and shrlgpa_levl_code = ''''U2''''
			  and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where ishrttrm.shrttrm_pidm = shrttrm.shrttrm_pidm and shrttrm_astd_code_end_of_term is not null )
			'')
		'
		exec(@tsql)

	end

	select * from #students

	drop table #students
GO
/****** Object:  View [dbo].[vUsers]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vUsers]
AS
SELECT     UserID AS id, LoginID, Email, Phone, FirstName, LastName, EmployeeID, SID, UserKey
FROM         Catbert3.dbo.Users
WHERE     (Inactive = 0)
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
         Begin Table = "Users (Catbert3.dbo)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 256
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
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vUsers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vUsers'
GO
/****** Object:  View [dbo].[vTermCodes]    Script Date: 10/05/2011 14:30:20 ******/
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
/****** Object:  Table [dbo].[Majors]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Majors](
	[id] [varchar](4) NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[DisciplineCode] [varchar](5) NULL,
	[CollegeCode] [char](2) NULL,
	[ConsolidationCode] [varchar](4) NULL,
	[IsActive] [bit] NOT NULL,
	[FullName] [varchar](100) NULL,
 CONSTRAINT [PK_Majors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TemplateTokens]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TemplateTokens](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TemplateTypeId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_TemplateTokens] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ceremonies]    Script Date: 10/05/2011 14:30:20 ******/
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
	[TotalStreamingTickets] [int] NULL,
	[PrintingDeadline] [date] NOT NULL,
	[RegistrationDeadline] [date] NOT NULL,
	[TermCode] [varchar](6) NOT NULL,
	[ExtraTicketDeadline] [date] NOT NULL,
	[ExtraTicketPerStudent] [int] NOT NULL,
	[MinUnits] [int] NOT NULL,
	[PetitionThreshold] [int] NOT NULL,
	[RegistrationBegin] [date] NOT NULL,
	[ExtraTicketBegin] [date] NOT NULL,
	[HasStreamingTickets] [bit] NOT NULL,
	[ConfirmationText] [varchar](max) NULL,
 CONSTRAINT [PK_Commencements] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Audits]    Script Date: 10/05/2011 14:30:20 ******/
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
/****** Object:  Table [dbo].[CeremonyMajors]    Script Date: 10/05/2011 14:30:20 ******/
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
/****** Object:  Table [dbo].[CeremonyEditors]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CeremonyEditors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CeremonyId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Owner] [bit] NOT NULL,
 CONSTRAINT [PK_CommencementEditors] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CeremonyColleges]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CeremonyColleges](
	[CeremonyId] [int] NOT NULL,
	[CollegeCode] [char](2) NOT NULL,
 CONSTRAINT [PK_CeremonyColleges] PRIMARY KEY CLUSTERED 
(
	[CeremonyId] ASC,
	[CollegeCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Templates]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Templates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[BodyText] [varchar](max) NOT NULL,
	[TemplateTypeId] [int] NOT NULL,
	[CeremonyId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Subject] [varchar](100) NULL,
 CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Students]    Script Date: 10/05/2011 14:30:20 ******/
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
	[MI] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[EarnedUnits] [decimal](6, 3) NULL,
	[CurrentUnits] [decimal](6, 3) NULL,
	[Email] [varchar](100) NULL,
	[Login] [varchar](50) NULL,
	[DateAdded] [datetime] NULL,
	[DateUpdated] [datetime] NULL,
	[TermCode] [varchar](6) NULL,
	[CeremonyId] [int] NULL,
	[SJABlock] [bit] NOT NULL,
	[Blocked] [bit] NOT NULL,
	[AddedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StudentMajors]    Script Date: 10/05/2011 14:30:20 ******/
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
/****** Object:  Table [dbo].[Registrations]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Registrations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [uniqueidentifier] NOT NULL,
	[Address1] [varchar](200) NOT NULL,
	[Address2] [varchar](200) NULL,
	[City] [varchar](100) NOT NULL,
	[State] [char](2) NOT NULL,
	[Zip] [varchar](15) NOT NULL,
	[Email] [varchar](100) NULL,
	[MailTickets] [bit] NOT NULL,
	[TermCode] [varchar](6) NOT NULL,
	[GradTrack] [bit] NOT NULL,
 CONSTRAINT [PK_Registrations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[usp_DownloadStudentsMultiCollege]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DownloadStudentsMultiCollege]
AS
    
IF object_id('tempdb..#Students') IS NOT NULL
BEGIN
    DROP TABLE #Students
END

CREATE TABLE #Students
(
    pidm varchar(8),
    studentid varchar(9),
    firstName varchar(50),
    MI varchar(50),
    LastName varchar(50),
    EarnedUnits decimal(6,3),
    CurrentUnits decimal(6,3),
    Email varchar(100),
    LoginId varchar(50),
    std varchar(2),
    major varchar(4)
)

declare @term varchar(6), @sisterm varchar(6), @minUnits int, @coll char(2)
declare @tsql varchar(max)

if (not exists (select * from termcodes where isactive = 1) )
begin
    return 1
end

set @term = (select MAX(id) from termcodes where isactive = 1)
select @sisterm = term from openquery(sis, '
											select min(stvterm_code) term
											from stvterm
											where stvterm_end_date > sysdate
											  and stvterm_trmt_code = ''Q''
										')
set @minUnits = (select min(PetitionThreshold) from Ceremonies where termcode = @term)    

if (GETDATE() + 8 < (select MIN(RegistrationBegin) from Ceremonies) or
	getdate() > (select MAX(registrationdeadline) from ceremonies) )
	begin
		return 2
	end
	
set @tsql = '
	insert into #students (pidm, studentid, firstname, mi, lastname, earnedunits, currentunits, email, loginid, std, major)
	select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name
		, earnedunits, currentunits, goremal_email_address, loginid, shrttrm_astd_code_end_of_term
		, zgvlcfs_majr_code
	from openquery(sis, ''
		select spriden_pidm, spriden_id, spriden_first_name, spriden_mi, spriden_last_name
			, EarnedUnits.shrlgpa_hours_earned as EarnedUnits
			, 0 as CurrentUnits
			, email.goremal_email_address
			, lower(wormoth_login_id) loginId
			, shrttrm_astd_code_end_of_term
			, zgvlcfs_majr_code
		from zgvlcfs
			inner join spriden on spriden_pidm = zgvlcfs_pidm
			inner join shrlgpa earnedUnits on earnedUnits.shrlgpa_pidm = zgvlcfs_pidm
			left outer join (
				select goremal_pidm, goremal_email_address
				from goremal
				where goremal_emal_code = ''''UCD''''
					and goremal_status_ind = ''''A''''
			) email on email.goremal_pidm = zgvlcfs_pidm
			inner join wormoth on wormoth_pidm = zgvlcfs_pidm
			left outer join shrttrm on shrttrm_pidm = zgvlcfs_pidm
		where spriden_change_ind is null
			and zgvlcfs_term_code_eff = '''''+@sisterm+'''''
			and EarnedUnits.shrlgpa_hours_earned > ' + CAST(@minUnits as varchar(6)) + '
			and shrttrm_term_code in ( select max(shrttrm_term_code) from shrttrm ishrttrm where shrttrm.shrttrm_pidm = ishrttrm.shrttrm_pidm )
			and wormoth_acct_type = ''''Z''''
			and wormoth_acct_status = ''''A''''
			and earnedUnits.shrlgpa_gpa_type_ind = ''''O''''
			and earnedUnits.shrlgpa_levL_code = ''''UG''''
	'')
'

exec (@tsql)

insert into Majors ( id, name, IsActive )
select distinct major, 'unknown', 0 from #Students
where major not in ( select id from Majors )

merge into students t
using (	select distinct pidm, studentid, firstname, mi
                    , lastname, earnedunits, currentUnits, email, loginid, @term as termcode
        from #students where std <> 'DS') s
on t.pidm = s.pidm and t.termcode = s.termcode
when matched then update
	-- only update the units
	set t.earnedunits = s.earnedunits, t.currentunits = s.currentunits, dateupdated = getdate()
when not matched then
    insert (pidm, studentid, firstname, mi, lastname, earnedunits, CurrentUnits, email, termcode, [login])
    values(s.pidm, s.studentid, s.firstname, s.mi, s.lastname, s.earnedunits, s.currentunits, s.email, s.termcode, s.[loginId]);

-- delete the student majors for which wee have an update for them
delete from StudentMajors
where Student_Id in ( select id from Students where studentId in ( select studentid from #students ) )

-- insert the updated student majors
insert into StudentMajors 
select distinct students.id, major from #students
        inner join students on #students.pidm = students.pidm and students.termcode = @term
    
DROP TABLE #Students

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_DownloadMissingMajors]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DownloadMissingMajors]
	
AS
	
	-- declare variables
	declare @term varchar(6), @id uniqueidentifier, @pidm varchar(7), @tsql varchar(max)

	-- create the temp table
	IF object_id('tempdb..#StudentMajors') IS NOT NULL
	BEGIN
		DROP TABLE #StudentMajors
	END

	CREATE TABLE #StudentMajors
	(
		id uniqueidentifier,
		major varchar(4)
	)

	-- get the current term
	set @term = (select MAX(id) from termcodes where isactive = 1)
																	
	declare @students cursor
	set @students = cursor for
		select id, pidm from students
		where id not in (select student_id from studentmajors)
		  and termcode = @term
	 
	open @students

	fetch next from @students into @id, @pidm	

	while(@@fetch_status = 0)
	begin
		delete from #StudentMajors

		set @tsql = '
			insert into #StudentMajors (major)
			select zgvlcfs_majr_code from openquery (sis, '' 
				select zgvlcfs_majr_code from zgvlcfs 
				where zgvlcfs_pidm = ''''' + @pidm + '''''
				  and zgvlcfs_levl_code in (''''UG'''', ''''U2'''')
				  and zgvlcfs_term_code_eff in ( select max(izgvlcfs.zgvlcfs_term_code_eff) from zgvlcfs izgvlcfs where zgvlcfs.zgvlcfs_pidm = izgvlcfs.zgvlcfs_pidm )
			'')
		'
		exec(@tsql)

		update #StudentMajors set id = @id
	
		merge studentmajors as t
		using #studentmajors as s
		on t.student_id = s.id and t.majorcode = s.major
		when not matched
		then insert (student_id, majorcode) values(s.id, s.major);
	
		fetch next from @students into @id, @pidm	

	end

	close @students
	deallocate @students

RETURN 0
GO
/****** Object:  Table [dbo].[RegistrationPetitions]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RegistrationPetitions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NOT NULL,
	[MajorCode] [varchar](4) NOT NULL,
	[ExceptionReason] [varchar](1000) NOT NULL,
	[CompletionTerm] [varchar](6) NOT NULL,
	[TransferUnitsFrom] [varchar](100) NULL,
	[TransferUnits] [varchar](5) NULL,
	[IsPending] [bit] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[DateSubmitted] [datetime] NOT NULL,
	[DateDecision] [datetime] NULL,
	[CeremonyId] [int] NOT NULL,
	[NumberTickets] [int] NOT NULL,
 CONSTRAINT [PK_RegistrationPetitions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RegistrationParticipations]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RegistrationParticipations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationId] [int] NOT NULL,
	[MajorCode] [varchar](4) NOT NULL,
	[CeremonyId] [int] NOT NULL,
	[NumberTickets] [int] NULL,
	[Cancelled] [bit] NULL,
	[LabelPrinted] [bit] NULL,
	[ExtraTicketPetitionId] [int] NULL,
	[DateRegistered] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
 CONSTRAINT [PK_RegistrationParticipations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RegistrationSpecialNeeds]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegistrationSpecialNeeds](
	[SpecialNeedId] [int] NOT NULL,
	[RegistrationId] [int] NOT NULL,
 CONSTRAINT [PK_RegistrationSpecialNeeds] PRIMARY KEY CLUSTERED 
(
	[SpecialNeedId] ASC,
	[RegistrationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailQueue]    Script Date: 10/05/2011 14:30:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailQueue](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [uniqueidentifier] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Pending] [bit] NOT NULL,
	[SentDateTime] [datetime] NULL,
	[TemplateId] [int] NOT NULL,
	[Subject] [varchar](100) NOT NULL,
	[Body] [varchar](max) NOT NULL,
	[Immediate] [bit] NOT NULL,
	[RegistrationId] [int] NULL,
	[RegistrationParticipationId] [int] NULL,
	[RegistrationPetitionId] [int] NULL,
	[ExtraTicketPetitionId] [int] NULL,
	[ErrorCode] [int] NULL,
 CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[udf_GetSpecialNeedsCSV]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alan Lai
-- Create date: 1/27/2011	
-- Description:	Takes a list of values from a table to
--				return a set csv
-- =============================================
CREATE FUNCTION [dbo].[udf_GetSpecialNeedsCSV]
(
	@id int	-- Registration Id
)
RETURNS varchar(max)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @results varchar(max)

	select @results = coalesce(@results + ', ', '') + sn.Name
	from RegistrationSpecialNeeds rpn 
		inner join SpecialNeeds sn on rpn.SpecialNeedId = sn.id
	where rpn.RegistrationId = @id

	-- Return the result of the function
	return @results

END
GO
/****** Object:  StoredProcedure [dbo].[usp_TotalRegistrationPetitions]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_TotalRegistrationPetitions]
	@term varchar(6),
	@userid int
AS
	
select students.LastName, students.FirstName, students.StudentId, rp.MajorCode Major
	, Ceremonies.[DateTime] as CeremonyTime
	, rp.ExceptionReason
	, case when rp.IsPending = 1 then 'Pending' when rp.IsPending = 0 and rp.IsApproved = 1 then 'Approved'
		else 'Denied'
	  end as PetitionStatus
	, COUNT(case when rp.IsPending = 1 then rp.id end) as TotalPendingPetitions	
	, COUNT(case when rp.IsPending = 0 and rp.IsApproved = 1 then rp.id end) as TotalApprovedPetitions
	, COUNT(case when rp.IsPending = 0 and rp.IsApproved = 0 then rp.id end) as TotalDeniedPetitions
	, TermCodes.Name as Term
from RegistrationPetitions rp
	inner join Registrations reg on rp.RegistrationId = reg.id
	inner join Students on students.Id = reg.Student_Id
	inner join Ceremonies on Ceremonies.id = rp.CeremonyId
	inner join TermCodes on students.termcode = termcodes.id
where students.TermCode = @term
  and students.SJABlock = 0
  and rp.CeremonyId in ( select CeremonyId from Ceremonies
							inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
						 where UserId = @userid
						   and TermCode = @term)						   
group by students.LastName, students.FirstName, students.StudentId, rp.MajorCode, Ceremonies.[DateTime]
	, rp.ExceptionReason, rp.IsPending, rp.IsApproved, TermCodes.Name
order by students.LastName

--SELECT     RegistrationPetitions.LastName, RegistrationPetitions.FirstName, RegistrationPetitions.StudentId, Majors.Name AS Major, Ceremonies.[DateTime] AS CeremonyTime,
--                      RegistrationPetitions.ExceptionReason, 
--                      CASE WHEN RegistrationPetitions.IsPending = 1 THEN 'Pending' WHEN RegistrationPetitions.IsPending = 0 AND 
--                      RegistrationPetitions.IsApproved = 1 THEN 'Approved' ELSE 'Denied' END AS PetitionStatus, 
--                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 1 THEN RegistrationPetitions.id END) AS TotalPendingPetitions, 
--                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 0 AND RegistrationPetitions.IsApproved = 1 THEN RegistrationPetitions.id END) AS TotalApprovedPetitions, 
--                      COUNT(CASE WHEN RegistrationPetitions.IsPending = 0 AND RegistrationPetitions.IsApproved = 0 THEN RegistrationPetitions.id END) AS TotalDeniedPetitions, 
--                      TermCodes.Name AS Term
--from RegistrationPetitions
--	inner join TermCodes on RegistrationPetitions.TermCode = TermCodes.id
--	inner join Majors on Majors.id = RegistrationPetitions.MajorCode
--	inner join Ceremonies on Ceremonies.id = RegistrationPetitions.CeremonyId
--WHERE RegistrationPetitions.CeremonyId in 
--		(select CeremonyId from Ceremonies
--			inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--		where UserId = @userid
--			and TermCode = @term)
--GROUP BY RegistrationPetitions.LastName, RegistrationPetitions.FirstName, RegistrationPetitions.StudentId, Majors.Name, Ceremonies.DateTime, 
--                      RegistrationPetitions.ExceptionReason, RegistrationPetitions.IsPending, RegistrationPetitions.IsApproved, TermCodes.Name

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_TotalRegisteredStudents]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_TotalRegisteredStudents]
	@term varchar(6),
	@userid int
AS

select s.lastname, s.firstname, s.studentid, rp.majorcode major
	, r.address1, r.address2, r.city, r.[state], r.zip, s.email as PrimaryEmail
	, r.email as SecondaryEmail, rp.numbertickets
	, etp.numbertickets as ExtraTickets, etp.numberticketsstreaming as ExtraStreamingTickets
	, case
		when etp.numbertickets is null then rp.numbertickets
		else etp.numbertickets + rp.numbertickets
		end as totaltickets
		, s.termcode as term
		, case when r.mailtickets = 1 then 'Mail' else 'Pickup' end as DistributionMethod
	, rp.dateregistered
	, c.datetime as CeremonyTime
from registrationparticipations rp
	inner join registrations r on rp.registrationid = r.id
	inner join Students s on r.student_id = s.id
	left outer join extraticketpetitions etp on etp.id = rp.extraticketpetitionid and etp.ispending = 1 and etp.isapproved = 1
	inner join ceremonies c on c.id = rp.ceremonyid
where r.termcode = @term
  and rp.ceremonyid in (select ceremonyid from ceremonyeditors where userid = @userid)
  and s.sjablock = 0
  and rp.cancelled = 0
order by s.lastname


--SELECT     Students.LastName, Students.FirstName, Students.StudentId, Majors.Name AS Major, Registrations.Address1, Registrations.Address2, Registrations.City, 
--                      Registrations.State, Registrations.Zip, Students.Email AS PrimaryEmail, Registrations.Email AS SecondaryEmail, Registrations.NumberTickets, 
--                      ExtraTicketPetitions.NumberTickets AS ExtraTicketPetitions, Registrations.MailTickets, Ceremonies.DateTime AS CeremonyTime, 
--                      CASE 
--						WHEN ExtraTicketPetitions.NumberTickets IS NULL 
--						THEN Registrations.NumberTickets 
--						ELSE ExtraTicketPetitions.NumberTickets + Registrations.NumberTickets 
--					END AS Tickets, 
--					SUM(Registrations.NumberTickets) 
--                      AS RegistrationTickets, 
--					  SUM((CASE WHEN ((ExtraTicketPetitions.NumberTickets IS NULL)) 
--								THEN Registrations.NumberTickets 
--								ELSE Registrations.NumberTickets + ExtraTicketPetitions.NumberTickets END)) 
--								AS TotalTickets
--					, Students.TermCode, TermCodes.Name AS Term, CASE WHEN MailTickets = 1 THEN 'Mail' ELSE 'Pickup' END AS DistributionMethod,
--					  Registrations.DateRegistered
--FROM         Registrations INNER JOIN
--                      Students ON Students.Id = Registrations.Student_Id LEFT OUTER JOIN
--                      ExtraTicketPetitions ON ExtraTicketPetitions.id = Registrations.ExtraTicketPetitionId AND ExtraTicketPetitions.IsApproved = 1 AND 
--                      ExtraTicketPetitions.IsPending = 0 INNER JOIN
--                      Ceremonies ON Ceremonies.id = Registrations.CeremonyId INNER JOIN
--                      Majors ON Majors.id = Registrations.MajorCode INNER JOIN
--                      TermCodes ON Ceremonies.TermCode = TermCodes.id
--WHERE     (Registrations.SJABlock = 0) and registrations.cancelled = 0
--	and Ceremonies.id in 
--			(select CeremonyId from Ceremonies
--				inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--			where UserId = @userid
--				and TermCode = @term)
--GROUP BY Students.LastName, Students.FirstName, Students.StudentId, Majors.Name, Registrations.Address1, Registrations.Address2, Registrations.City, 
--                      Registrations.State, Registrations.Zip, Students.Email, Registrations.Email, Registrations.NumberTickets, ExtraTicketPetitions.NumberTickets, 
--                      Registrations.MailTickets, Ceremonies.DateTime, Students.TermCode, TermCodes.Name
--HAVING      (Students.TermCode = @term)

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_TotalRegisteredByMajor]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_TotalRegisteredByMajor]
	@term varchar(6),
	@userid int,
	@major varchar(4)
AS

select s.lastname, s.firstname, s.studentid, rp.majorcode major
	, r.address1, r.address2, r.city, r.[state], r.zip, s.email as PrimaryEmail
	, r.email as SecondaryEmail, rp.numbertickets
	, etp.numbertickets as ExtraTickets, etp.numberticketsstreaming as ExtraStreamingTickets
	, case
		when etp.numbertickets is null then rp.numbertickets
		else etp.numbertickets + rp.numbertickets
		end as totaltickets
		, s.termcode as term
		, case when r.mailtickets = 1 then 'Mail' else 'Pickup' end as DistributionMethod
	, rp.dateregistered
	, c.datetime as CeremonyTime
from registrationparticipations rp
	inner join registrations r on rp.registrationid = r.id
	inner join Students s on r.student_id = s.id
	left outer join extraticketpetitions etp on etp.id = rp.extraticketpetitionid and etp.ispending = 1 and etp.isapproved = 1
	inner join ceremonies c on c.id = rp.ceremonyid
where r.termcode = @term
  and rp.ceremonyid in (select ceremonyid from ceremonyeditors where userid = @userid)
  and s.sjablock = 0
  and rp.cancelled = 0
  and rp.majorcode = @major
order by s.lastname



RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_TicketSignOutSheet]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_TicketSignOutSheet]
	@term varchar(6),
	@userid int
AS
	
	/*
		List of students who have decided to pickup their tickets
	*/

	select students.LastName, students.FirstName, students.StudentId, rp.MajorCode Major
		, rp.NumberTickets RegistrationTickets
		, ISNULL(etp.NumberTickets, 0) ExtraTickets
		, ISNULL(etp.NumberTicketsStreaming, 0) ExtraStreamingTickets
	from RegistrationParticipations rp
		inner join Registrations reg on rp.RegistrationId = reg.id
		inner join Students on students.Id = reg.Student_Id
		left outer join ExtraTicketPetitions etp on etp.id = rp.ExtraTicketPetitionId and etp.IsPending = 1 and etp.IsApproved = 1
	where students.TermCode = @term
	  and reg.MailTickets = 0
	  and students.SJABlock = 0
	  and rp.Cancelled = 0
	  and rp.CeremonyId in ( select CeremonyId from Ceremonies
								inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
							 where UserId = @userid
							   and TermCode = @term)

	--select students.LastName, students.FirstName, students.StudentId, Majors.Name Major
	-- , Registrations.NumberTickets RegistrationTickets
	-- , ISNULL(ExtraTicketPetitions.numbertickets, 0) ExtraTickets
	-- , Registrations.CeremonyId		
	--from Registrations
	--	left outer join ExtraTicketPetitions on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id and ExtraTicketPetitions.IsPending = 0 and ExtraTicketPetitions.IsApproved = 1
	--	inner join Students on Registrations.Student_Id = students.Id
	--	inner join Majors on Registrations.MajorCode = Majors.id
	--where Students.TermCode = @term
	--	and Registrations.MailTickets = 0
	--	and Registrations.SJABlock = 0
	--	and Registrations.Cancelled = 0
	--	and Registrations.CeremonyId in 
	--		(select CeremonyId from Ceremonies
	--		inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
	--		where UserId = @userid
	--		and TermCode = @term)
	--order by LastName


RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_SummaryReport]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SummaryReport]
	@term varchar(6),
	@userid int
AS

/*
	Provides a summary of all registrations
*/

select ceremonies.id, ceremonies.DateTime, ceremonies.TermCode
	, ceremonies.totaltickets totalceremonytickets
	, isnull(ceremonies.totalstreamingtickets, 0) TotalStreamingTickets
	, registrationtickets.tickets RegistrationTickets
	, isnull(pendingextratickets.tickets, 0) PendingExtraTickets
	, isnull(pendingextratickets.streamingtickets, 0) PendingStreamingExtraTickets
	, isnull(extratickets.tickets, 0) ApprovedExtraTickets
	, isnull(extratickets.streamingtickets, 0) ApprovedStreamingExtraTickets
from ceremonies
	left outer join (
		select sum(rp.numbertickets) tickets, rp.ceremonyid
		from registrationparticipations rp
			inner join registrations r on rp.registrationid = r.id
			inner join students s on r.student_id = s.id
		where rp.cancelled = 0
		  and s.sjablock = 0 and s.blocked = 0
		group by rp.ceremonyid
	) RegistrationTickets on RegistrationTickets.CeremonyId = ceremonies.id
	left outer join (
		select sum(etp.numbertickets) tickets, sum(etp.numberticketsstreaming) streamingtickets, ceremonyid
		from extraticketpetitions etp
			inner join registrationparticipations rp on rp.extraticketpetitionid = etp.id
		where etp.ispending = 0 and etp.isapproved = 1 and rp.cancelled = 0
		group by rp.ceremonyid
	) ExtraTickets on ExtraTickets.CeremonyId = ceremonies.id
	left outer join (
		select sum(isnull(etp.numbertickets, etp.numberticketsrequested)) tickets
			 , sum(isnull(etp.numberticketsstreaming, etp.numberticketsrequestedstreaming)) streamingtickets
			 , ceremonyid
		from extraticketpetitions etp
			inner join registrationparticipations rp on rp.extraticketpetitionid = etp.id
		where etp.ispending = 1 and rp.Cancelled = 0
		group by rp.ceremonyid
	) PendingExtraTickets on PendingExtraTickets.CeremonyId = ceremonies.id
where ceremonies.id in ( select CeremonyId from CeremonyEditors 
									inner join Ceremonies on CeremonyEditors.CeremonyId = Ceremonies.id
						 where UserId = @userId
						   and Ceremonies.TermCode = @term
					    )	


--select Ceremonies.id ceremonyid, Ceremonies.[DateTime], Ceremonies.TermCode, Ceremonies.TotalTickets totalceremonytickets
--	, RegistrationTickets.NumberTickets RegistrationTickets
--	, ISNULL(PendingExtraTickets.NumberTickets, 0) PendingExtraTickets
--	, ISNULL(ApprovedExtraTickets.NumberTickets, 0) ApprovedExtraTickets
--from Ceremonies
--	left outer join (
--		select sum(NumberTickets) NumberTickets, registrations.CeremonyId from Registrations
--			inner join Students on Registrations.Student_Id = students.Id
--		where Registrations.SJABlock = 0
--			and Registrations.Cancelled = 0
--			and students.TermCode = @term
--		group by Registrations.CeremonyId
--	) RegistrationTickets on RegistrationTickets.ceremonyid = ceremonies.id
--	left outer join (
--		select SUM(ExtraTicketPetitions.NumberTickets) NumberTickets, Registrations.CeremonyId from ExtraTicketPetitions
--			inner join Registrations on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id
--			inner join Students on Registrations.Student_Id = students.Id
--		where Registrations.SJABlock = 0
--			and Registrations.Cancelled = 0
--			and students.TermCode = @term
--			and ExtraTicketPetitions.IsPending = 1
--		group by Registrations.CeremonyId
--	) PendingExtraTickets on PendingExtraTickets.ceremonyid = ceremonies.id
--	left outer join (
--		select SUM(ExtraTicketPetitions.NumberTickets) NumberTickets, Registrations.CeremonyId from ExtraTicketPetitions
--			inner join Registrations on Registrations.ExtraTicketPetitionId = ExtraTicketPetitions.id
--			inner join Students on Registrations.Student_Id = students.Id
--		where Registrations.SJABlock = 0
--			and Registrations.Cancelled = 0
--			and students.TermCode = @term
--			and ExtraTicketPetitions.IsPending = 0
--			and ExtraTicketPetitions.IsApproved = 1
--		group by Registrations.CeremonyId
--	) ApprovedExtraTickets on ApprovedExtraTickets.ceremonyid = ceremonies.id	
--where ceremonies.id in (select CeremonyId from Ceremonies
--							inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--						where UserId = @userid
--							and TermCode = @term)


RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_MajorCountByCeremony]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_MajorCountByCeremony]
	@ceremonyId int, 
	@userId int
AS

/*
	Count of students by major
*/

select count(rp.id) num, isnull(m.fullname, m.name + '[Banner Name]') name, c.datetime
from registrationparticipations rp
	inner join registrations r on r.id = rp.registrationid
	inner join students s on r.student_id = s.id
	inner join majors m on rp.majorcode = m.id
	inner join ceremonies c on rp.ceremonyid = c.id
where rp.ceremonyid = 1
  and cancelled = 0
  and sjablock = 0 and blocked = 0
group by m.fullname, m.name, c.datetime
order by m.name

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_RegistrarReport]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_RegistrarReport]
	@term varchar(6),
	@userId int
AS

/*
	Registrar requires a list of all participating students
*/

--select students.StudentId, students.LastName, students.FirstName, students.MI, Majors.Name Major
--from Registrations
--	inner join Students on Registrations.Student_Id = students.Id
--	inner join Majors on Registrations.MajorCode = Majors.id
--where Registrations.SJABlock = 0
--	and Registrations.Cancelled = 0
--	and Registrations.CeremonyId in (select CeremonyId from Ceremonies
--									inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--								where UserId = @userid
--									and TermCode = @term)
--order by students.lastname

select stud.StudentId, stud.LastName, stud.FirstName, stud.MI, rp.MajorCode
from RegistrationParticipations rp
	inner join Registrations reg on rp.RegistrationId = reg.id
	inner join Students stud on reg.Student_Id = stud.Id
where stud.SJABlock = 0
  and rp.Cancelled = 0
  and rp.CeremonyId in ( select CeremonyId from CeremonyEditors 
									inner join Ceremonies on CeremonyEditors.CeremonyId = Ceremonies.id
						 where UserId = @userId
						   and Ceremonies.TermCode = @term
					    )
order by stud.LastName

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_ProcessMailing]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ProcessMailing]
	@immediate bit
AS
	
	declare @subject varchar(100), @body varchar(max), @emails varchar(max), @queueId int
	declare @automatedEmail varchar(50) = 'automatedemail@caes.ucdavis.edu'
	declare @queue cursor

	set @queue = cursor for
		select emailqueue.id, [subject], [body]
			,case 
				when registrations.email is null then students.email
				else students.email + ';' + registrations.email
				end as emails
		from emailqueue
			inner join Students on students.Id = EmailQueue.Student_Id
			inner join Registrations on registrations.id = EmailQueue.RegistrationId
		where Pending = 1
		  and [immediate] = @immediate
	
	open @queue

	fetch next from @queue into @queueId, @subject, @body, @emails

	while (@@FETCH_STATUS = 0)
	begin
	
		exec msdb.dbo.sp_send_dbmail
			@recipients = @emails,
			@blind_copy_recipients = @automatedEmail,
			@subject = @subject,
			@body = @body,
			@body_format = 'HTML';
		
		update emailqueue
		set errorcode = @@ERROR, SentDateTime = GETDATE(), Pending = 0
		where id = @queueId

		fetch next from @queue into @queueId, @subject, @body, @emails
	end

	close @queue
	deallocate @queue

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[usp_SpecialNeedsReport]    Script Date: 10/05/2011 14:30:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SpecialNeedsReport]
    @term varchar(6),
    @userid int
AS

/*
	Returns a list of students with special needs marked
*/
    
--SELECT     Students.LastName, Students.FirstName, Students.StudentId, Majors.Name AS Major, Students.Email AS PrimaryEmail, Registrations.Email AS SecondaryEmail, 
--                      Ceremonies.DateTime AS CeremonyTime, Registrations.Comments AS SpecialNeeds, TermCodes.Name AS Term,
--                      ceremonies.id
--FROM         Registrations INNER JOIN
--                      Students ON Students.Id = Registrations.Student_Id INNER JOIN
--                      Ceremonies ON Ceremonies.id = Registrations.CeremonyId INNER JOIN
--                      Majors ON Majors.id = Registrations.MajorCode INNER JOIN
--                      TermCodes ON Ceremonies.TermCode = TermCodes.id
--WHERE   (Students.TermCode = @term) 
--	AND (Registrations.Comments IS NOT NULL) 
--	AND (LEN(Registrations.Comments) > 0) 
--	AND (Registrations.SJABlock = 0)
--	AND (Registrations.Cancelled = 0)
--	and Ceremonies.id in (select CeremonyId from Ceremonies
--								inner join ceremonyeditors on ceremonies.id = ceremonyeditors.CeremonyId
--							where UserId = @userid
--								and TermCode = @term)

select students.LastName, students.FirstName, students.StudentId
	, rp.MajorCode as Major
	, students.Email as PrimaryEmail
	, reg.Email as SecondaryEmail
	, ceremonies.DateTime as CeremonyTime
	, dbo.udf_GetSpecialNeedsCSV(reg.id) as SpecialNeeds
	, TermCodes.Name as Term
	, Ceremonies.id
from Registrations reg
	inner join Students on students.Id = reg.Student_Id
	inner join RegistrationParticipations rp on rp.RegistrationId = reg.id
	inner join Ceremonies on rp.CeremonyId = Ceremonies.id
	inner join TermCodes on Ceremonies.TermCode = termcodes.id
where students.TermCode = @term
  and students.SJABlock = 0
  and rp.Cancelled = 0
  and rp.CeremonyId in ( select CeremonyId from CeremonyEditors 
									inner join Ceremonies on CeremonyEditors.CeremonyId = Ceremonies.id
						 where UserId = @userId
						   and Ceremonies.TermCode = @term
					    )
  and dbo.udf_GetSpecialNeedsCSV(reg.id) is not null					    
order by Ceremonies.DateTime, students.LastName

RETURN 0
GO
/****** Object:  Default [DF_Ceremonies_HasStreamingTickets]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Ceremonies] ADD  CONSTRAINT [DF_Ceremonies_HasStreamingTickets]  DEFAULT ((0)) FOR [HasStreamingTickets]
GO
/****** Object:  Default [DF_CommencementEditors_Owner]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[CeremonyEditors] ADD  CONSTRAINT [DF_CommencementEditors_Owner]  DEFAULT ((0)) FOR [Owner]
GO
/****** Object:  Default [DF_Colleges_Display]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Colleges] ADD  CONSTRAINT [DF_Colleges_Display]  DEFAULT ((0)) FOR [Display]
GO
/****** Object:  Default [DF_EmailQueue_Pending]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[EmailQueue] ADD  CONSTRAINT [DF_EmailQueue_Pending]  DEFAULT ((1)) FOR [Pending]
GO
/****** Object:  Default [DF_EmailQueue_Immediate]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[EmailQueue] ADD  CONSTRAINT [DF_EmailQueue_Immediate]  DEFAULT ((0)) FOR [Immediate]
GO
/****** Object:  Default [DF_ExtraTicketPetitions_IsPending]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[ExtraTicketPetitions] ADD  CONSTRAINT [DF_ExtraTicketPetitions_IsPending]  DEFAULT ((1)) FOR [IsPending]
GO
/****** Object:  Default [DF_ExtraTicketPetitions_IsApproved]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[ExtraTicketPetitions] ADD  CONSTRAINT [DF_ExtraTicketPetitions_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
/****** Object:  Default [DF_ExtraTicketPetitions_DateSubmitted]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[ExtraTicketPetitions] ADD  CONSTRAINT [DF_ExtraTicketPetitions_DateSubmitted]  DEFAULT (getdate()) FOR [DateSubmitted]
GO
/****** Object:  Default [DF_ExtraTicketPetitions_LabelPrinted]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[ExtraTicketPetitions] ADD  CONSTRAINT [DF_ExtraTicketPetitions_LabelPrinted]  DEFAULT ((0)) FOR [LabelPrinted]
GO
/****** Object:  Default [DF_Majors_IsActive]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Majors] ADD  CONSTRAINT [DF_Majors_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_PageTracking_DateTime]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[PageTracking] ADD  CONSTRAINT [DF_PageTracking_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO
/****** Object:  Default [DF_PageTracking_IsEmulating]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[PageTracking] ADD  CONSTRAINT [DF_PageTracking_IsEmulating]  DEFAULT ((0)) FOR [IsEmulating]
GO
/****** Object:  Default [DF_RegistrationParticipations_Cancelled]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationParticipations] ADD  CONSTRAINT [DF_RegistrationParticipations_Cancelled]  DEFAULT ((0)) FOR [Cancelled]
GO
/****** Object:  Default [DF_RegistrationParticipations_LabelPrinted]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationParticipations] ADD  CONSTRAINT [DF_RegistrationParticipations_LabelPrinted]  DEFAULT ((0)) FOR [LabelPrinted]
GO
/****** Object:  Default [DF_RegistrationParticipations_DateRegistered]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationParticipations] ADD  CONSTRAINT [DF_RegistrationParticipations_DateRegistered]  DEFAULT (getdate()) FOR [DateRegistered]
GO
/****** Object:  Default [DF_RegistrationPetitions_IsPending]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationPetitions] ADD  CONSTRAINT [DF_RegistrationPetitions_IsPending]  DEFAULT ((1)) FOR [IsPending]
GO
/****** Object:  Default [DF_RegistrationPetitions_IsApproved]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationPetitions] ADD  CONSTRAINT [DF_RegistrationPetitions_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
/****** Object:  Default [DF_RegistrationPetitions_DateSubmitted]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationPetitions] ADD  CONSTRAINT [DF_RegistrationPetitions_DateSubmitted]  DEFAULT (getdate()) FOR [DateSubmitted]
GO
/****** Object:  Default [DF_Registrations_MailTickets]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Registrations] ADD  CONSTRAINT [DF_Registrations_MailTickets]  DEFAULT ((0)) FOR [MailTickets]
GO
/****** Object:  Default [DF_Registrations_GradTrack]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Registrations] ADD  CONSTRAINT [DF_Registrations_GradTrack]  DEFAULT ((0)) FOR [GradTrack]
GO
/****** Object:  Default [DF_SpecialNeeds_IsActive]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[SpecialNeeds] ADD  CONSTRAINT [DF_SpecialNeeds_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Students_Id]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_Id]  DEFAULT (newid()) FOR [Id]
GO
/****** Object:  Default [DF_Students_DatedAdded]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_DatedAdded]  DEFAULT (getdate()) FOR [DateAdded]
GO
/****** Object:  Default [DF_Students_DateUpdated]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_DateUpdated]  DEFAULT (getdate()) FOR [DateUpdated]
GO
/****** Object:  Default [DF_Students_SJABlock]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_SJABlock]  DEFAULT ((0)) FOR [SJABlock]
GO
/****** Object:  Default [DF_Students_Removed]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_Removed]  DEFAULT ((0)) FOR [Blocked]
GO
/****** Object:  Default [DF_Templates_IsActive]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Templates] ADD  CONSTRAINT [DF_Templates_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_TermCodes_IsActive]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[TermCodes] ADD  CONSTRAINT [DF_TermCodes_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
/****** Object:  ForeignKey [FK_Audits_ActionCodes1]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Audits]  WITH CHECK ADD  CONSTRAINT [FK_Audits_ActionCodes1] FOREIGN KEY([AuditActionTypeId])
REFERENCES [dbo].[AuditActionTypes] ([ID])
GO
ALTER TABLE [dbo].[Audits] CHECK CONSTRAINT [FK_Audits_ActionCodes1]
GO
/****** Object:  ForeignKey [FK_Commencements_TermCodes]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Ceremonies]  WITH CHECK ADD  CONSTRAINT [FK_Commencements_TermCodes] FOREIGN KEY([TermCode])
REFERENCES [dbo].[TermCodes] ([id])
GO
ALTER TABLE [dbo].[Ceremonies] CHECK CONSTRAINT [FK_Commencements_TermCodes]
GO
/****** Object:  ForeignKey [FK_CeremonyColleges_Ceremonies]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[CeremonyColleges]  WITH CHECK ADD  CONSTRAINT [FK_CeremonyColleges_Ceremonies] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[CeremonyColleges] CHECK CONSTRAINT [FK_CeremonyColleges_Ceremonies]
GO
/****** Object:  ForeignKey [FK_CeremonyColleges_Colleges]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[CeremonyColleges]  WITH CHECK ADD  CONSTRAINT [FK_CeremonyColleges_Colleges] FOREIGN KEY([CollegeCode])
REFERENCES [dbo].[Colleges] ([id])
GO
ALTER TABLE [dbo].[CeremonyColleges] CHECK CONSTRAINT [FK_CeremonyColleges_Colleges]
GO
/****** Object:  ForeignKey [FK_CommencementEditors_Commencements]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[CeremonyEditors]  WITH CHECK ADD  CONSTRAINT [FK_CommencementEditors_Commencements] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[CeremonyEditors] CHECK CONSTRAINT [FK_CommencementEditors_Commencements]
GO
/****** Object:  ForeignKey [FK_CeremonyMajors_Majors]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[CeremonyMajors]  WITH CHECK ADD  CONSTRAINT [FK_CeremonyMajors_Majors] FOREIGN KEY([MajorCode])
REFERENCES [dbo].[Majors] ([id])
GO
ALTER TABLE [dbo].[CeremonyMajors] CHECK CONSTRAINT [FK_CeremonyMajors_Majors]
GO
/****** Object:  ForeignKey [FK_CommencementMajors_Commencements]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[CeremonyMajors]  WITH CHECK ADD  CONSTRAINT [FK_CommencementMajors_Commencements] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[CeremonyMajors] CHECK CONSTRAINT [FK_CommencementMajors_Commencements]
GO
/****** Object:  ForeignKey [FK_EmailQueue_ExtraTicketPetitions]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[EmailQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmailQueue_ExtraTicketPetitions] FOREIGN KEY([ExtraTicketPetitionId])
REFERENCES [dbo].[ExtraTicketPetitions] ([id])
GO
ALTER TABLE [dbo].[EmailQueue] CHECK CONSTRAINT [FK_EmailQueue_ExtraTicketPetitions]
GO
/****** Object:  ForeignKey [FK_EmailQueue_RegistrationParticipations]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[EmailQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmailQueue_RegistrationParticipations] FOREIGN KEY([RegistrationParticipationId])
REFERENCES [dbo].[RegistrationParticipations] ([id])
GO
ALTER TABLE [dbo].[EmailQueue] CHECK CONSTRAINT [FK_EmailQueue_RegistrationParticipations]
GO
/****** Object:  ForeignKey [FK_EmailQueue_RegistrationPetitions]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[EmailQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmailQueue_RegistrationPetitions] FOREIGN KEY([RegistrationPetitionId])
REFERENCES [dbo].[RegistrationPetitions] ([id])
GO
ALTER TABLE [dbo].[EmailQueue] CHECK CONSTRAINT [FK_EmailQueue_RegistrationPetitions]
GO
/****** Object:  ForeignKey [FK_EmailQueue_Registrations]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[EmailQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmailQueue_Registrations] FOREIGN KEY([RegistrationId])
REFERENCES [dbo].[Registrations] ([id])
GO
ALTER TABLE [dbo].[EmailQueue] CHECK CONSTRAINT [FK_EmailQueue_Registrations]
GO
/****** Object:  ForeignKey [FK_EmailQueue_Students]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[EmailQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmailQueue_Students] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[EmailQueue] CHECK CONSTRAINT [FK_EmailQueue_Students]
GO
/****** Object:  ForeignKey [FK_EmailQueue_Templates]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[EmailQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmailQueue_Templates] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[Templates] ([id])
GO
ALTER TABLE [dbo].[EmailQueue] CHECK CONSTRAINT [FK_EmailQueue_Templates]
GO
/****** Object:  ForeignKey [FK_Majors_Colleges]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Majors]  WITH CHECK ADD  CONSTRAINT [FK_Majors_Colleges] FOREIGN KEY([CollegeCode])
REFERENCES [dbo].[Colleges] ([id])
GO
ALTER TABLE [dbo].[Majors] CHECK CONSTRAINT [FK_Majors_Colleges]
GO
/****** Object:  ForeignKey [FK_Majors_Majors]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Majors]  WITH CHECK ADD  CONSTRAINT [FK_Majors_Majors] FOREIGN KEY([ConsolidationCode])
REFERENCES [dbo].[Majors] ([id])
GO
ALTER TABLE [dbo].[Majors] CHECK CONSTRAINT [FK_Majors_Majors]
GO
/****** Object:  ForeignKey [FK_RegistrationParticipations_Ceremonies]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationParticipations]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationParticipations_Ceremonies] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[RegistrationParticipations] CHECK CONSTRAINT [FK_RegistrationParticipations_Ceremonies]
GO
/****** Object:  ForeignKey [FK_RegistrationParticipations_ExtraTicketPetitions]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationParticipations]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationParticipations_ExtraTicketPetitions] FOREIGN KEY([ExtraTicketPetitionId])
REFERENCES [dbo].[ExtraTicketPetitions] ([id])
GO
ALTER TABLE [dbo].[RegistrationParticipations] CHECK CONSTRAINT [FK_RegistrationParticipations_ExtraTicketPetitions]
GO
/****** Object:  ForeignKey [FK_RegistrationParticipations_Registrations]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationParticipations]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationParticipations_Registrations] FOREIGN KEY([RegistrationId])
REFERENCES [dbo].[Registrations] ([id])
GO
ALTER TABLE [dbo].[RegistrationParticipations] CHECK CONSTRAINT [FK_RegistrationParticipations_Registrations]
GO
/****** Object:  ForeignKey [FK_RegistrationPetitions_Ceremonies]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationPetitions]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationPetitions_Ceremonies] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[RegistrationPetitions] CHECK CONSTRAINT [FK_RegistrationPetitions_Ceremonies]
GO
/****** Object:  ForeignKey [FK_RegistrationPetitions_RegistrationPetitions]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationPetitions]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationPetitions_RegistrationPetitions] FOREIGN KEY([RegistrationId])
REFERENCES [dbo].[Registrations] ([id])
GO
ALTER TABLE [dbo].[RegistrationPetitions] CHECK CONSTRAINT [FK_RegistrationPetitions_RegistrationPetitions]
GO
/****** Object:  ForeignKey [FK_Registrations_Registrations]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_Registrations] FOREIGN KEY([id])
REFERENCES [dbo].[Registrations] ([id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_Registrations]
GO
/****** Object:  ForeignKey [FK_Registrations_States]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_States] FOREIGN KEY([State])
REFERENCES [dbo].[States] ([Id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_States]
GO
/****** Object:  ForeignKey [FK_Registrations_Students]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_Students] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_Students]
GO
/****** Object:  ForeignKey [FK_Registrations_TermCodes]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_TermCodes] FOREIGN KEY([TermCode])
REFERENCES [dbo].[TermCodes] ([id])
GO
ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_TermCodes]
GO
/****** Object:  ForeignKey [FK_RegistrationSpecialNeeds_Registrations]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationSpecialNeeds]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationSpecialNeeds_Registrations] FOREIGN KEY([RegistrationId])
REFERENCES [dbo].[Registrations] ([id])
GO
ALTER TABLE [dbo].[RegistrationSpecialNeeds] CHECK CONSTRAINT [FK_RegistrationSpecialNeeds_Registrations]
GO
/****** Object:  ForeignKey [FK_RegistrationSpecialNeeds_SpecialNeeds]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[RegistrationSpecialNeeds]  WITH CHECK ADD  CONSTRAINT [FK_RegistrationSpecialNeeds_SpecialNeeds] FOREIGN KEY([SpecialNeedId])
REFERENCES [dbo].[SpecialNeeds] ([id])
GO
ALTER TABLE [dbo].[RegistrationSpecialNeeds] CHECK CONSTRAINT [FK_RegistrationSpecialNeeds_SpecialNeeds]
GO
/****** Object:  ForeignKey [FK_StudentMajors_Majors]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[StudentMajors]  WITH CHECK ADD  CONSTRAINT [FK_StudentMajors_Majors] FOREIGN KEY([MajorCode])
REFERENCES [dbo].[Majors] ([id])
GO
ALTER TABLE [dbo].[StudentMajors] CHECK CONSTRAINT [FK_StudentMajors_Majors]
GO
/****** Object:  ForeignKey [FK_StudentMajors_Students]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[StudentMajors]  WITH CHECK ADD  CONSTRAINT [FK_StudentMajors_Students] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Students] ([Id])
GO
ALTER TABLE [dbo].[StudentMajors] CHECK CONSTRAINT [FK_StudentMajors_Students]
GO
/****** Object:  ForeignKey [FK_Students_Ceremonies]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Ceremonies] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Ceremonies]
GO
/****** Object:  ForeignKey [FK_Templates_Ceremonies]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Templates]  WITH CHECK ADD  CONSTRAINT [FK_Templates_Ceremonies] FOREIGN KEY([CeremonyId])
REFERENCES [dbo].[Ceremonies] ([id])
GO
ALTER TABLE [dbo].[Templates] CHECK CONSTRAINT [FK_Templates_Ceremonies]
GO
/****** Object:  ForeignKey [FK_Templates_TemplateTypes]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[Templates]  WITH CHECK ADD  CONSTRAINT [FK_Templates_TemplateTypes] FOREIGN KEY([TemplateTypeId])
REFERENCES [dbo].[TemplateTypes] ([id])
GO
ALTER TABLE [dbo].[Templates] CHECK CONSTRAINT [FK_Templates_TemplateTypes]
GO
/****** Object:  ForeignKey [FK_TemplateTokens_TemplateTypes]    Script Date: 10/05/2011 14:30:20 ******/
ALTER TABLE [dbo].[TemplateTokens]  WITH CHECK ADD  CONSTRAINT [FK_TemplateTokens_TemplateTypes] FOREIGN KEY([TemplateTypeId])
REFERENCES [dbo].[TemplateTypes] ([id])
GO
ALTER TABLE [dbo].[TemplateTokens] CHECK CONSTRAINT [FK_TemplateTokens_TemplateTypes]
GO

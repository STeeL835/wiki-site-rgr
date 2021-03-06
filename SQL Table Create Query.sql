USE [Wiki-site DB]
GO
/****** Object:  Table [dbo].[ArticleComments]    Script Date: 10.06.2017 14:25:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleComments](
	[Id] [uniqueidentifier] NOT NULL,
	[Article_Id] [uniqueidentifier] NOT NULL,
	[Author_Id] [uniqueidentifier] NOT NULL,
	[Text] [nvarchar](1500) NOT NULL,
	[Date_Of_Creation] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ArticleComments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ArticleContents]    Script Date: 10.06.2017 14:25:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleContents](
	[Id] [uniqueidentifier] NOT NULL,
	[Heading] [nvarchar](50) NOT NULL,
	[Definition] [nvarchar](300) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[Main_Image] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ArticleContent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Articles]    Script Date: 10.06.2017 14:25:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[Id] [uniqueidentifier] NOT NULL,
	[Short_Url] [nvarchar](max) NOT NULL,
	[Author_Id] [uniqueidentifier] NOT NULL,
	[Date_Of_Creation] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ArticleVersions]    Script Date: 10.06.2017 14:25:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleVersions](
	[Id] [uniqueidentifier] NOT NULL,
	[Article_Id] [uniqueidentifier] NOT NULL,
	[Content_Id] [uniqueidentifier] NOT NULL,
	[Editor_Id] [uniqueidentifier] NOT NULL,
	[Date_Of_Edition] [datetime2](7) NOT NULL,
	[Is_Approved] [bit] NOT NULL,
 CONSTRAINT [PK_ArticleVersions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Credentials]    Script Date: 10.06.2017 14:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Credentials](
	[Id] [uniqueidentifier] NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password_Hash] [binary](64) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Credentials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Images]    Script Date: 10.06.2017 14:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[Id] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Images_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10.06.2017 14:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[sysdiagrams]    Script Date: 10.06.2017 14:25:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysdiagrams](
	[name] [sysname] NOT NULL,
	[principal_id] [int] NOT NULL,
	[diagram_id] [int] IDENTITY(1,1) NOT NULL,
	[version] [int] NULL,
	[definition] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[diagram_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
 CONSTRAINT [UK_principal_name] UNIQUE NONCLUSTERED 
(
	[principal_id] ASC,
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Users]    Script Date: 10.06.2017 14:25:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Short_Id] [int] IDENTITY(1,1) NOT NULL,
	[Credentials_Id] [uniqueidentifier] NOT NULL,
	[Nickname] [nvarchar](50) NOT NULL,
	[About] [nvarchar](500) NULL,
	[Role_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[VersionVotes]    Script Date: 10.06.2017 14:25:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VersionVotes](
	[Id] [uniqueidentifier] NOT NULL,
	[ArticleVersion_Id] [uniqueidentifier] NOT NULL,
	[User_Id] [uniqueidentifier] NOT NULL,
	[Vote] [bit] NOT NULL,
 CONSTRAINT [PK_VersionVotes_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
ALTER TABLE [dbo].[ArticleComments] ADD  CONSTRAINT [DF_ArticleComments_Author_Id]  DEFAULT ('61bbaea5-e7ea-432f-87a8-ccda7f3a842b') FOR [Author_Id]
GO
ALTER TABLE [dbo].[Articles] ADD  CONSTRAINT [DF_Articles_Author_Id]  DEFAULT ('61bbaea5-e7ea-432f-87a8-ccda7f3a842b') FOR [Author_Id]
GO
ALTER TABLE [dbo].[ArticleVersions] ADD  CONSTRAINT [DF_ArticleVersions_Editor_Id]  DEFAULT ('61bbaea5-e7ea-432f-87a8-ccda7f3a842b') FOR [Editor_Id]
GO
ALTER TABLE [dbo].[ArticleVersions] ADD  CONSTRAINT [DF_ArticleVersions_Is_Approved]  DEFAULT ((0)) FOR [Is_Approved]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Role_Id]  DEFAULT ('d905f4f9-4ff6-4164-b12d-9c5ce0b5b561') FOR [Role_Id]
GO
ALTER TABLE [dbo].[ArticleComments]  WITH CHECK ADD  CONSTRAINT [FK_ArticleComments_Articles] FOREIGN KEY([Article_Id])
REFERENCES [dbo].[Articles] ([Id])
GO
ALTER TABLE [dbo].[ArticleComments] CHECK CONSTRAINT [FK_ArticleComments_Articles]
GO
ALTER TABLE [dbo].[ArticleComments]  WITH CHECK ADD  CONSTRAINT [FK_ArticleComments_Users] FOREIGN KEY([Author_Id])
REFERENCES [dbo].[Users] ([Id])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[ArticleComments] CHECK CONSTRAINT [FK_ArticleComments_Users]
GO
ALTER TABLE [dbo].[ArticleContents]  WITH CHECK ADD  CONSTRAINT [FK_ArticleContents_Images] FOREIGN KEY([Main_Image])
REFERENCES [dbo].[Images] ([Id])
GO
ALTER TABLE [dbo].[ArticleContents] CHECK CONSTRAINT [FK_ArticleContents_Images]
GO
ALTER TABLE [dbo].[Articles]  WITH CHECK ADD  CONSTRAINT [FK_Articles_Users] FOREIGN KEY([Author_Id])
REFERENCES [dbo].[Users] ([Id])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[Articles] CHECK CONSTRAINT [FK_Articles_Users]
GO
ALTER TABLE [dbo].[ArticleVersions]  WITH CHECK ADD  CONSTRAINT [FK_ArticleVersions_ArticleContent] FOREIGN KEY([Content_Id])
REFERENCES [dbo].[ArticleContents] ([Id])
GO
ALTER TABLE [dbo].[ArticleVersions] CHECK CONSTRAINT [FK_ArticleVersions_ArticleContent]
GO
ALTER TABLE [dbo].[ArticleVersions]  WITH CHECK ADD  CONSTRAINT [FK_ArticleVersions_Articles] FOREIGN KEY([Article_Id])
REFERENCES [dbo].[Articles] ([Id])
GO
ALTER TABLE [dbo].[ArticleVersions] CHECK CONSTRAINT [FK_ArticleVersions_Articles]
GO
ALTER TABLE [dbo].[ArticleVersions]  WITH CHECK ADD  CONSTRAINT [FK_ArticleVersions_Users] FOREIGN KEY([Editor_Id])
REFERENCES [dbo].[Users] ([Id])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[ArticleVersions] CHECK CONSTRAINT [FK_ArticleVersions_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Credentials] FOREIGN KEY([Credentials_Id])
REFERENCES [dbo].[Credentials] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Credentials]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([Role_Id])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE SET DEFAULT
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[VersionVotes]  WITH CHECK ADD  CONSTRAINT [FK_VersionVotes_ArticleVersions] FOREIGN KEY([ArticleVersion_Id])
REFERENCES [dbo].[ArticleVersions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VersionVotes] CHECK CONSTRAINT [FK_VersionVotes_ArticleVersions]
GO
ALTER TABLE [dbo].[VersionVotes]  WITH CHECK ADD  CONSTRAINT [FK_VersionVotes_Users] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VersionVotes] CHECK CONSTRAINT [FK_VersionVotes_Users]
GO

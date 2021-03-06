USE [ecommercedat]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 13.08.2021 22:43:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryTrans]    Script Date: 13.08.2021 22:43:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryTrans](
	[CategoryId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[Translation] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 13.08.2021 22:43:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 13.08.2021 22:43:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[CategoryId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTrans]    Script Date: 13.08.2021 22:43:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductTrans](
	[ProductId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[Translation] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 13.08.2021 22:43:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Toys')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (2, N'School')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
INSERT [dbo].[CategoryTrans] ([CategoryId], [LanguageId], [Translation]) VALUES (1, 1, N'Oyuncaklar')
INSERT [dbo].[CategoryTrans] ([CategoryId], [LanguageId], [Translation]) VALUES (2, 1, N'Okul')
GO
INSERT [dbo].[Languages] ([Id], [Name]) VALUES (1, N'Türkçe')
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [Name], [CategoryId]) VALUES (2, N'Teddy Bear', 1)
INSERT [dbo].[Products] ([Id], [Name], [CategoryId]) VALUES (3, N'Car', 1)
INSERT [dbo].[Products] ([Id], [Name], [CategoryId]) VALUES (4, N'Water Gun', 1)
INSERT [dbo].[Products] ([Id], [Name], [CategoryId]) VALUES (5, N'Pencil', 2)
INSERT [dbo].[Products] ([Id], [Name], [CategoryId]) VALUES (6, N'Notebook', 2)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
INSERT [dbo].[ProductTrans] ([ProductId], [LanguageId], [Translation]) VALUES (2, 1, N'Oyuncak Ayi')
INSERT [dbo].[ProductTrans] ([ProductId], [LanguageId], [Translation]) VALUES (3, 1, N'Araba')
INSERT [dbo].[ProductTrans] ([ProductId], [LanguageId], [Translation]) VALUES (4, 1, N'Su Tabancasi')
INSERT [dbo].[ProductTrans] ([ProductId], [LanguageId], [Translation]) VALUES (5, 1, N'Kalem')
INSERT [dbo].[ProductTrans] ([ProductId], [LanguageId], [Translation]) VALUES (6, 1, N'Defter')
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Username], [Password]) VALUES (4, N'alamutq', N'asdasdasd')
INSERT [dbo].[Users] ([Id], [Username], [Password]) VALUES (1, N'asd', N'123')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[CategoryTrans]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[CategoryTrans]  WITH CHECK ADD FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Languages] ([Id])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[ProductTrans]  WITH CHECK ADD FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Languages] ([Id])
GO
ALTER TABLE [dbo].[ProductTrans]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO

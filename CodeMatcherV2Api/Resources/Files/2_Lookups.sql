Delete from [dbo].[Lookups]

SET IDENTITY_INSERT [dbo].[Lookups] ON 

INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (1, 1, N'School')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (2, 1, N'Hospital')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (3, 1, N'Insurance')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (4, 1, N'State License')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (5, 2, N'Triggered')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (6, 2, N'Scheduled')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (7, 3, N'Code Generation')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (8, 3, N'Weekly Embedding')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (9, 3, N'Monthly Embedding')
INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (10, 2, N'Upload Csv')
SET IDENTITY_INSERT [dbo].[Lookups] OFF

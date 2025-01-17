GO
SET IDENTITY_INSERT [dbo].[LookupTypes] ON 

INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (1, N'Segment', N'This defines the type of segment from which data being extracted.')
INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (2, N'RunType', N'This defines the type of run user wanted i.e. Triggered or Scheduled.')
INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (3, N'CodeMappingType', N'This defines the type of code mapping user selected(Ex: CodeGeneration, WeeklyEmbeddings, MonthlyEmbeddings)')
SET IDENTITY_INSERT [dbo].[LookupTypes] OFF
GO

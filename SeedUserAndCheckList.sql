USE [MemberTrack]

GO
SET IDENTITY_INSERT [dbo].[PersonCheckListItem] ON 

INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (1, 6, N'Welcome email from elder group', 6, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (2, 1, N'Send "Thank you for visiting" email from Connections', 1, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (3, 3, N'Did Connections call?', 3, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (4, 5, N'Welcome email from Connections', 5, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (5, 3, N'Did Children’s Minister call (With infant, toddler or elemtary age)?', 3, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (6, 7, N'Email Life Group invitation', 7, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (7, 2, N'Email from Mic', 2, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (8, 4, N'Did minister call?', 4, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (9, 3, N'Did Duncan call (Junior / High School)', 3, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (10, 1, N'Send email from Children’s Ministry (With infant, toddler or elementary age kids)', 1, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (11, 7, N'Did they join Life Group?', 7, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (12, 1, N'Send email from Duncan (With  Junior High / Highschoolers)', 1, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (13, 1, N'Send Welcome booklet', 1, 1)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (14, 5, N'Link to Family Information for Servant Keeper membership software', 5, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (15, 5, N'Welcome email from Mic', 5, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (16, 5, N'Welcome email from Children''s Ministry / Duncan', 5, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (17, 8, N'Were they emailed Ministry Service information link?', 8, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (18, 8, N'Did they fill out Ministry Service information?', 8, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (19, 8, N'Were ministry leaders told areas they are interested in?', 8, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (20, 8, N'Did they connect with a ministry leader?', 8, 2)
INSERT [dbo].[PersonCheckListItem] ([Id], [CheckListItemType], [Description], [SortOrder], [Status]) VALUES (21, 8, N'Were their gifts added to Servant Keeper?', 8, 2)
SET IDENTITY_INSERT [dbo].[PersonCheckListItem] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [DisplayName], [Email], [Password], [Role]) VALUES (-1989, N'Alberto De Pena', N'adepena@nbchurchfamily.org', N'XZDRUEcVwG++2GKU0zZKl2X0tSyj+FmHlAHBWPJ5sEk=', 4)
INSERT [dbo].[User] ([Id], [DisplayName], [Email], [Password], [Role]) VALUES (-1971, N'David Maltby', N'dmaltby@nbchurchfamily.org', N'tjWdEz3003Oxgcfjt6WG6XFQUX6HhHg/I0o+MkCr4wM=', 4)
SET IDENTITY_INSERT [dbo].[User] OFF

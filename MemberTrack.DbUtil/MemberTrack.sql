IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [Document] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(350),
        [Extension] nvarchar(5) NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Size] bigint NOT NULL,
        CONSTRAINT [PK_Document] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [Person] (
        [Id] bigint NOT NULL IDENTITY,
        [AgeGroup] int NOT NULL,
        [BaptismDate] datetimeoffset,
        [ContactNumber] nvarchar(15),
        [Email] nvarchar(256),
        [FirstName] nvarchar(75) NOT NULL,
        [Gender] int NOT NULL,
        [LastName] nvarchar(75) NOT NULL,
        [MembershipDate] datetimeoffset,
        [MiddleName] nvarchar(75),
        [Status] int NOT NULL,
        CONSTRAINT [PK_Person] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [PersonCheckListItem] (
        [Id] bigint NOT NULL IDENTITY,
        [CheckListItemType] int NOT NULL,
        [Description] nvarchar(300) NOT NULL,
        CONSTRAINT [PK_PersonCheckListItem] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [User] (
        [Id] bigint NOT NULL IDENTITY,
        [DisplayName] nvarchar(256) NOT NULL,
        [Email] nvarchar(256) NOT NULL,
        [Password] nvarchar(256) NOT NULL,
        [Role] int NOT NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [DocumentData] (
        [DocumentId] bigint NOT NULL,
        [Data] varbinary(max) NOT NULL,
        CONSTRAINT [PK_DocumentData] PRIMARY KEY ([DocumentId]),
        CONSTRAINT [FK_DocumentData_Document_DocumentId] FOREIGN KEY ([DocumentId]) REFERENCES [Document] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [DocumentTag] (
        [Id] bigint NOT NULL IDENTITY,
        [DocumentId] bigint NOT NULL,
        [Value] nvarchar(75) NOT NULL,
        CONSTRAINT [PK_DocumentTag] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DocumentTag_Document_DocumentId] FOREIGN KEY ([DocumentId]) REFERENCES [Document] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [Address] (
        [PersonId] bigint NOT NULL,
        [City] nvarchar(150) NOT NULL,
        [State] int NOT NULL,
        [Street] nvarchar(150) NOT NULL,
        [ZipCode] int NOT NULL,
        CONSTRAINT [PK_Address] PRIMARY KEY ([PersonId]),
        CONSTRAINT [FK_Address_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [ChildrenInfo] (
        [Id] bigint NOT NULL IDENTITY,
        [AgeGroup] int NOT NULL,
        [PersonId] bigint NOT NULL,
        CONSTRAINT [PK_ChildrenInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ChildrenInfo_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE TABLE [PersonCheckList] (
        [PersonId] bigint NOT NULL,
        [PersonCheckListItemId] bigint NOT NULL,
        [Date] datetimeoffset NOT NULL,
        [Note] nvarchar(500),
        CONSTRAINT [PK_PersonCheckList] PRIMARY KEY ([PersonId], [PersonCheckListItemId]),
        CONSTRAINT [FK_PersonCheckList_PersonCheckListItem_PersonCheckListItemId] FOREIGN KEY ([PersonCheckListItemId]) REFERENCES [PersonCheckListItem] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PersonCheckList_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE UNIQUE INDEX [IX_Address_PersonId] ON [Address] ([PersonId]) WHERE [PersonId] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE INDEX [IX_ChildrenInfo_PersonId] ON [ChildrenInfo] ([PersonId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE UNIQUE INDEX [IX_DocumentData_DocumentId] ON [DocumentData] ([DocumentId]) WHERE [DocumentId] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE INDEX [IX_DocumentTag_DocumentId] ON [DocumentTag] ([DocumentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE UNIQUE INDEX [IX_Person_Email] ON [Person] ([Email]) WHERE [Email] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE INDEX [IX_PersonCheckList_PersonCheckListItemId] ON [PersonCheckList] ([PersonCheckListItemId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE INDEX [IX_PersonCheckList_PersonId] ON [PersonCheckList] ([PersonId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE UNIQUE INDEX [IX_PersonCheckListItem_Description] ON [PersonCheckListItem] ([Description]) WHERE [Description] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    CREATE UNIQUE INDEX [IX_User_Email] ON [User] ([Email]) WHERE [Email] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161230171811_CreateDB')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20161230171811_CreateDB', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170203175347_RemoveChildrenInfoTable')
BEGIN
    DROP TABLE [ChildrenInfo];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170203175347_RemoveChildrenInfoTable')
BEGIN
    ALTER TABLE [Person] ADD [HasElementaryKids] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170203175347_RemoveChildrenInfoTable')
BEGIN
    ALTER TABLE [Person] ADD [HasHighSchoolKids] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170203175347_RemoveChildrenInfoTable')
BEGIN
    ALTER TABLE [Person] ADD [HasInfantKids] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170203175347_RemoveChildrenInfoTable')
BEGIN
    ALTER TABLE [Person] ADD [HasJuniorHighKids] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170203175347_RemoveChildrenInfoTable')
BEGIN
    ALTER TABLE [Person] ADD [HasToddlerKids] bit NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170203175347_RemoveChildrenInfoTable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170203175347_RemoveChildrenInfoTable', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170216222741_RemoveDocumentTables')
BEGIN
    DROP TABLE [DocumentData];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170216222741_RemoveDocumentTables')
BEGIN
    DROP TABLE [DocumentTag];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170216222741_RemoveDocumentTables')
BEGIN
    DROP TABLE [Document];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170216222741_RemoveDocumentTables')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170216222741_RemoveDocumentTables', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170220204701_PersonFirstVisitDate')
BEGIN
    ALTER TABLE [Person] ADD [FirstVisitDate] datetimeoffset;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170220204701_PersonFirstVisitDate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170220204701_PersonFirstVisitDate', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170228142412_CheckListItemSortOrder')
BEGIN
    ALTER TABLE [PersonCheckListItem] ADD [SortOrder] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170228142412_CheckListItemSortOrder')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170228142412_CheckListItemSortOrder', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170228160441_PersonCreatedDate')
BEGIN
    ALTER TABLE [Person] ADD [CreatedDate] datetimeoffset NOT NULL DEFAULT '0001-01-01T00:00:00.000+00:00';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170228160441_PersonCreatedDate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170228160441_PersonCreatedDate', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170228161040_DefaultCreatedDate')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Person') AND [c].[name] = N'CreatedDate');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Person] DROP CONSTRAINT [' + @var0 + ']');
    ALTER TABLE [Person] ALTER COLUMN [CreatedDate] datetimeoffset NOT NULL;
    ALTER TABLE [Person] ADD DEFAULT (GETUTCDATE()) FOR [CreatedDate];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170228161040_DefaultCreatedDate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170228161040_DefaultCreatedDate', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE TABLE [Quiz] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(300),
        [Instructions] nvarchar(max),
        [Name] nvarchar(50) NOT NULL,
        [RandomizeQuestions] bit NOT NULL,
        CONSTRAINT [PK_Quiz] PRIMARY KEY ([Id]),
        CONSTRAINT [AlternateKey_Name] UNIQUE ([Name])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE TABLE [QuizTopicCategory] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(300),
        [Name] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_QuizTopicCategory] PRIMARY KEY ([Id]),
        CONSTRAINT [AK_QuizTopicCategory_Name] UNIQUE ([Name])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE TABLE [QuizQuestion] (
        [Id] bigint NOT NULL IDENTITY,
        [AllowMultipleAnswers] bit NOT NULL,
        [Description] nvarchar(300) NOT NULL,
        [QuizId] bigint NOT NULL,
        [RandomizeAnswers] bit NOT NULL,
        CONSTRAINT [PK_QuizQuestion] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizQuestion_Quiz_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quiz] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE TABLE [QuizTopic] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(300),
        [Name] nvarchar(50) NOT NULL,
        [TopicCategoryId] bigint NOT NULL,
        CONSTRAINT [PK_QuizTopic] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizTopic_QuizTopicCategory_TopicCategoryId] FOREIGN KEY ([TopicCategoryId]) REFERENCES [QuizTopicCategory] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE TABLE [QuizAnswer] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(150) NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        [QuestionId] bigint NOT NULL,
        [TopicId] bigint,
        [TopicWeight] int NOT NULL,
        CONSTRAINT [PK_QuizAnswer] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizAnswer_QuizQuestion_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [QuizQuestion] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_QuizAnswer_QuizTopic_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [QuizTopic] ([Id]) ON DELETE SET NULL
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE TABLE [QuizSupportingScripture] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(300),
        [ScriptureReference] nvarchar(50) NOT NULL,
        [TopicId] bigint NOT NULL,
        CONSTRAINT [PK_QuizSupportingScripture] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizSupportingScripture_QuizTopic_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [QuizTopic] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE TABLE [QuizUserAnswer] (
        [Id] bigint NOT NULL IDENTITY,
        [AnswerId] bigint NOT NULL,
        [UserId] bigint NOT NULL,
        CONSTRAINT [PK_QuizUserAnswer] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizUserAnswer_QuizAnswer_AnswerId] FOREIGN KEY ([AnswerId]) REFERENCES [QuizAnswer] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_QuizUserAnswer_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE INDEX [IX_QuizAnswer_QuestionId] ON [QuizAnswer] ([QuestionId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE INDEX [IX_QuizAnswer_TopicId] ON [QuizAnswer] ([TopicId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE INDEX [IX_QuizQuestion_QuizId] ON [QuizQuestion] ([QuizId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE INDEX [IX_QuizSupportingScripture_TopicId] ON [QuizSupportingScripture] ([TopicId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE INDEX [IX_QuizTopic_TopicCategoryId] ON [QuizTopic] ([TopicCategoryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE INDEX [IX_QuizUserAnswer_AnswerId] ON [QuizUserAnswer] ([AnswerId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    CREATE INDEX [IX_QuizUserAnswer_UserId] ON [QuizUserAnswer] ([UserId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303024119_QuizEntitiesCreated')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170303024119_QuizEntitiesCreated', N'1.0.0-rtm-21431');
END;

GO


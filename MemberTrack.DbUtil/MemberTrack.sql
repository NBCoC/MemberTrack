IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE TABLE [Person] (
        [Id] bigint NOT NULL IDENTITY,
        [AgeGroup] int NOT NULL,
        [CreatedDate] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
        [Email] nvarchar(256),
        [FirstVisitDate] datetimeoffset,
        [FullName] nvarchar(150) NOT NULL,
        [MembershipDate] datetimeoffset,
        [Status] int NOT NULL,
        CONSTRAINT [PK_Person] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE TABLE [PersonCheckListItem] (
        [Id] bigint NOT NULL IDENTITY,
        [CheckListItemType] int NOT NULL,
        [Description] nvarchar(300) NOT NULL,
        [SortOrder] int NOT NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_PersonCheckListItem] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE TABLE [QuizPersonAnswer] (
        [Id] bigint NOT NULL IDENTITY,
        [AnswerId] bigint NOT NULL,
        [PersonId] bigint NOT NULL,
        CONSTRAINT [PK_QuizPersonAnswer] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizPersonAnswer_QuizAnswer_AnswerId] FOREIGN KEY ([AnswerId]) REFERENCES [QuizAnswer] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_QuizPersonAnswer_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE UNIQUE INDEX [IX_Person_Email] ON [Person] ([Email]) WHERE [Email] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_PersonCheckList_PersonCheckListItemId] ON [PersonCheckList] ([PersonCheckListItemId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_PersonCheckList_PersonId] ON [PersonCheckList] ([PersonId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE UNIQUE INDEX [IX_PersonCheckListItem_Description] ON [PersonCheckListItem] ([Description]) WHERE [Description] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_QuizAnswer_QuestionId] ON [QuizAnswer] ([QuestionId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_QuizAnswer_TopicId] ON [QuizAnswer] ([TopicId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_QuizPersonAnswer_AnswerId] ON [QuizPersonAnswer] ([AnswerId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_QuizPersonAnswer_PersonId] ON [QuizPersonAnswer] ([PersonId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_QuizQuestion_QuizId] ON [QuizQuestion] ([QuizId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_QuizSupportingScripture_TopicId] ON [QuizSupportingScripture] ([TopicId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE INDEX [IX_QuizTopic_TopicCategoryId] ON [QuizTopic] ([TopicCategoryId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    CREATE UNIQUE INDEX [IX_User_Email] ON [User] ([Email]) WHERE [Email] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170323205435_CreateDb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170323205435_CreateDb', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170418162409_PersonContactNumber')
BEGIN
    DROP INDEX [IX_Person_Email] ON [Person];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170418162409_PersonContactNumber')
BEGIN
    ALTER TABLE [Person] ADD [ContactNumber] nvarchar(15);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170418162409_PersonContactNumber')
BEGIN
    ALTER TABLE [Person] ADD [Description] nvarchar(500);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170418162409_PersonContactNumber')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170418162409_PersonContactNumber', N'1.0.0-rtm-21431');
END;

GO


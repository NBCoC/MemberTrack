IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE TABLE [VisitorCheckListItem] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(300),
        [VisitType] int NOT NULL,
        CONSTRAINT [PK_VisitorCheckListItem] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE TABLE [DocumentData] (
        [DocumentId] bigint NOT NULL,
        [Data] varbinary(max) NOT NULL,
        CONSTRAINT [PK_DocumentData] PRIMARY KEY ([DocumentId]),
        CONSTRAINT [FK_DocumentData_Document_DocumentId] FOREIGN KEY ([DocumentId]) REFERENCES [Document] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE TABLE [ChildrenInfo] (
        [PersonId] bigint NOT NULL,
        [AgeGroup] int NOT NULL,
        CONSTRAINT [PK_ChildrenInfo] PRIMARY KEY ([PersonId]),
        CONSTRAINT [FK_ChildrenInfo_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE TABLE [Visit] (
        [VisitorId] bigint NOT NULL,
        [Date] datetimeoffset NOT NULL,
        [Note] nvarchar(300),
        CONSTRAINT [PK_Visit] PRIMARY KEY ([VisitorId]),
        CONSTRAINT [FK_Visit_Person_VisitorId] FOREIGN KEY ([VisitorId]) REFERENCES [Person] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE TABLE [VisitorCheckList] (
        [VisitorId] bigint NOT NULL,
        [VisitorCheckListItemId] bigint NOT NULL,
        CONSTRAINT [PK_VisitorCheckList] PRIMARY KEY ([VisitorId], [VisitorCheckListItemId]),
        CONSTRAINT [FK_VisitorCheckList_VisitorCheckListItem_VisitorCheckListItemId] FOREIGN KEY ([VisitorCheckListItemId]) REFERENCES [VisitorCheckListItem] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_VisitorCheckList_Visit_VisitorId] FOREIGN KEY ([VisitorId]) REFERENCES [Visit] ([VisitorId]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE UNIQUE INDEX [IX_Address_PersonId] ON [Address] ([PersonId]) WHERE [PersonId] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE INDEX [IX_ChildrenInfo_PersonId] ON [ChildrenInfo] ([PersonId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE UNIQUE INDEX [IX_DocumentData_DocumentId] ON [DocumentData] ([DocumentId]) WHERE [DocumentId] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE INDEX [IX_DocumentTag_DocumentId] ON [DocumentTag] ([DocumentId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE UNIQUE INDEX [IX_Person_Email] ON [Person] ([Email]) WHERE [Email] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE UNIQUE INDEX [IX_User_Email] ON [User] ([Email]) WHERE [Email] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE INDEX [IX_Visit_VisitorId] ON [Visit] ([VisitorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE INDEX [IX_VisitorCheckList_VisitorCheckListItemId] ON [VisitorCheckList] ([VisitorCheckListItemId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    CREATE INDEX [IX_VisitorCheckList_VisitorId] ON [VisitorCheckList] ([VisitorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161206225529_Create')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20161206225529_Create', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207041750_VisitorUpdate')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'VisitorCheckListItem') AND [c].[name] = N'VisitType');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [VisitorCheckListItem] DROP CONSTRAINT [' + @var0 + ']');
    ALTER TABLE [VisitorCheckListItem] DROP COLUMN [VisitType];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207041750_VisitorUpdate')
BEGIN
    ALTER TABLE [Visit] ADD [VisitType] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207041750_VisitorUpdate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20161207041750_VisitorUpdate', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051256_GroupVisitor')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'Visit') AND [c].[name] = N'VisitType');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Visit] DROP CONSTRAINT [' + @var1 + ']');
    ALTER TABLE [Visit] DROP COLUMN [VisitType];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051256_GroupVisitor')
BEGIN
    ALTER TABLE [VisitorCheckListItem] ADD [Group] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051256_GroupVisitor')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'VisitorCheckListItem') AND [c].[name] = N'Description');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [VisitorCheckListItem] DROP CONSTRAINT [' + @var2 + ']');
    ALTER TABLE [VisitorCheckListItem] ALTER COLUMN [Description] nvarchar(300) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051256_GroupVisitor')
BEGIN
    CREATE UNIQUE INDEX [IX_VisitorCheckListItem_Description] ON [VisitorCheckListItem] ([Description]) WHERE [Description] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051256_GroupVisitor')
BEGIN
    CREATE UNIQUE INDEX [IX_Visit_Date] ON [Visit] ([Date]) WHERE [Date] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051256_GroupVisitor')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20161207051256_GroupVisitor', N'1.0.0-rtm-21431');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051728_RenameVisitor')
BEGIN
    DROP TABLE [VisitorCheckList];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051728_RenameVisitor')
BEGIN
    DROP TABLE [VisitorCheckListItem];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051728_RenameVisitor')
BEGIN
    CREATE TABLE [VisitCheckListItem] (
        [Id] bigint NOT NULL IDENTITY,
        [Description] nvarchar(300) NOT NULL,
        [Group] int NOT NULL,
        CONSTRAINT [PK_VisitCheckListItem] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051728_RenameVisitor')
BEGIN
    CREATE TABLE [VisitCheckList] (
        [VisitorId] bigint NOT NULL,
        [VisitCheckListItemId] bigint NOT NULL,
        CONSTRAINT [PK_VisitCheckList] PRIMARY KEY ([VisitorId], [VisitCheckListItemId]),
        CONSTRAINT [FK_VisitCheckList_VisitCheckListItem_VisitCheckListItemId] FOREIGN KEY ([VisitCheckListItemId]) REFERENCES [VisitCheckListItem] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_VisitCheckList_Visit_VisitorId] FOREIGN KEY ([VisitorId]) REFERENCES [Visit] ([VisitorId]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051728_RenameVisitor')
BEGIN
    CREATE INDEX [IX_VisitCheckList_VisitCheckListItemId] ON [VisitCheckList] ([VisitCheckListItemId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051728_RenameVisitor')
BEGIN
    CREATE INDEX [IX_VisitCheckList_VisitorId] ON [VisitCheckList] ([VisitorId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051728_RenameVisitor')
BEGIN
    CREATE UNIQUE INDEX [IX_VisitCheckListItem_Description] ON [VisitCheckListItem] ([Description]) WHERE [Description] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20161207051728_RenameVisitor')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20161207051728_RenameVisitor', N'1.0.0-rtm-21431');
END;

GO


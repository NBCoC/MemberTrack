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


IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
BEGIN
    CREATE TABLE [Person] (
        [Id] bigint NOT NULL IDENTITY,
        [AgeGroup] int NOT NULL,
        [BaptismDate] datetimeoffset,
        [ContactNumber] nvarchar(15),
        [CreatedDate] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
        [Email] nvarchar(256),
        [FirstName] nvarchar(75) NOT NULL,
        [FirstVisitDate] datetimeoffset,
        [Gender] int NOT NULL,
        [HasElementaryKids] bit NOT NULL,
        [HasHighSchoolKids] bit NOT NULL,
        [HasInfantKids] bit NOT NULL,
        [HasJuniorHighKids] bit NOT NULL,
        [HasToddlerKids] bit NOT NULL,
        [LastName] nvarchar(75) NOT NULL,
        [MembershipDate] datetimeoffset,
        [MiddleName] nvarchar(75),
        [Status] int NOT NULL,
        CONSTRAINT [PK_Person] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
BEGIN
    CREATE UNIQUE INDEX [IX_Address_PersonId] ON [Address] ([PersonId]) WHERE [PersonId] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
BEGIN
    CREATE UNIQUE INDEX [IX_Person_Email] ON [Person] ([Email]) WHERE [Email] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
BEGIN
    CREATE INDEX [IX_PersonCheckList_PersonCheckListItemId] ON [PersonCheckList] ([PersonCheckListItemId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
BEGIN
    CREATE INDEX [IX_PersonCheckList_PersonId] ON [PersonCheckList] ([PersonId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
BEGIN
    CREATE UNIQUE INDEX [IX_PersonCheckListItem_Description] ON [PersonCheckListItem] ([Description]) WHERE [Description] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
BEGIN
    CREATE UNIQUE INDEX [IX_User_Email] ON [User] ([Email]) WHERE [Email] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20170303141943_CreateDb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20170303141943_CreateDb', N'1.0.0-rtm-21431');
END;

GO


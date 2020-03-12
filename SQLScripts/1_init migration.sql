IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200312105832_Init')
BEGIN
    CREATE TABLE [Locations] (
        [LocationId] int NOT NULL,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_Locations] PRIMARY KEY ([LocationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200312105832_Init')
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [Username] nvarchar(max) NULL,
        [Password] nvarchar(max) NULL,
        [Token] nvarchar(max) NULL,
        [Role] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200312105832_Init')
BEGIN
    CREATE TABLE [Population] (
        [Pointer] uniqueidentifier NOT NULL,
        [Indicator] int NOT NULL,
        [Frequency] int NOT NULL,
        [Age] int NOT NULL,
        [Sex] int NOT NULL,
        [LocationId] int NOT NULL,
        CONSTRAINT [PK_Population] PRIMARY KEY ([Pointer]),
        CONSTRAINT [FK_Population_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([LocationId]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200312105832_Init')
BEGIN
    CREATE INDEX [IX_Population_LocationId] ON [Population] ([LocationId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200312105832_Init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200312105832_Init', N'3.1.2');
END;

GO
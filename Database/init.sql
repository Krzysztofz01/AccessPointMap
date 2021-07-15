IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Users] (
    [Id] bigint NOT NULL IDENTITY,
    [AddDate] datetime2 NOT NULL DEFAULT (getdate()),
    [EditDate] datetime2 NOT NULL DEFAULT (getdate()),
    [DeleteDate] datetime2 NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(450) NULL,
    [Password] nvarchar(max) NOT NULL,
    [LastLoginIp] nvarchar(max) NOT NULL DEFAULT N'',
    [LastLoginDate] datetime2 NOT NULL DEFAULT (getdate()),
    [AdminPermission] bit NOT NULL DEFAULT CAST(0 AS bit),
    [WritePermission] bit NOT NULL DEFAULT CAST(0 AS bit),
    [ReadPermission] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsActivated] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AccessPoints] (
    [Id] bigint NOT NULL IDENTITY,
    [AddDate] datetime2 NOT NULL DEFAULT (getdate()),
    [EditDate] datetime2 NOT NULL DEFAULT (getdate()),
    [DeleteDate] datetime2 NOT NULL,
    [Bssid] nvarchar(max) NOT NULL,
    [Ssid] nvarchar(max) NOT NULL,
    [Fingerprint] nvarchar(max) NOT NULL,
    [Frequency] float NOT NULL,
    [MaxSignalLevel] int NOT NULL,
    [MaxSignalLongitude] float NOT NULL,
    [MaxSignalLatitude] float NOT NULL,
    [MinSignalLevel] int NOT NULL,
    [MinSignalLongitude] float NOT NULL,
    [MinSignalLatitude] float NOT NULL,
    [SignalRadius] float NOT NULL,
    [SignalArea] float NOT NULL,
    [FullSecurityData] nvarchar(max) NOT NULL,
    [SerializedSecurityData] nvarchar(max) NOT NULL,
    [Manufacturer] nvarchar(max) NULL,
    [DeviceType] nvarchar(max) NULL,
    [MasterGroup] bit NOT NULL DEFAULT CAST(0 AS bit),
    [Display] bit NOT NULL DEFAULT CAST(0 AS bit),
    [UserAddedId] bigint NULL,
    [UserModifiedId] bigint NULL,
    CONSTRAINT [PK_AccessPoints] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AccessPoints_Users_UserAddedId] FOREIGN KEY ([UserAddedId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_AccessPoints_Users_UserModifiedId] FOREIGN KEY ([UserModifiedId]) REFERENCES [Users] ([Id])
);

GO

CREATE INDEX [IX_AccessPoints_UserAddedId] ON [AccessPoints] ([UserAddedId]);

GO

CREATE INDEX [IX_AccessPoints_UserModifiedId] ON [AccessPoints] ([UserModifiedId]);

GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]) WHERE [Email] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210612150441_InitialMigration', N'3.1.16');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'ReadPermission');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] DROP COLUMN [ReadPermission];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'WritePermission');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] DROP COLUMN [WritePermission];

GO

ALTER TABLE [Users] ADD [ModPermission] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [AccessPoints] ADD [Note] nvarchar(max) NULL DEFAULT N'';

GO

CREATE TABLE [RefreshTokens] (
    [Id] bigint NOT NULL IDENTITY,
    [AddDate] datetime2 NOT NULL DEFAULT (getdate()),
    [EditDate] datetime2 NOT NULL DEFAULT (getdate()),
    [DeleteDate] datetime2 NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [Expires] datetime2 NOT NULL,
    [Created] datetime2 NOT NULL DEFAULT (getdate()),
    [CreatedByIp] nvarchar(max) NOT NULL DEFAULT N'',
    [Revoked] datetime2 NULL,
    [RevokedByIp] nvarchar(max) NOT NULL DEFAULT N'',
    [IsRevoked] bit NOT NULL DEFAULT CAST(0 AS bit),
    [ReplacedByToken] nvarchar(max) NULL,
    [UserId] bigint NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
);

GO

CREATE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210616090033_RefreshTokensAndBugFix', N'3.1.16');

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'DeleteDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] ALTER COLUMN [DeleteDate] datetime2 NULL;

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RefreshTokens]') AND [c].[name] = N'DeleteDate');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [RefreshTokens] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [RefreshTokens] ALTER COLUMN [DeleteDate] datetime2 NULL;

GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AccessPoints]') AND [c].[name] = N'DeleteDate');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AccessPoints] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [AccessPoints] ALTER COLUMN [DeleteDate] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210616093028_NullableFieldFix', N'3.1.16');

GO

ALTER TABLE [AccessPoints] ADD [IsHidden] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [AccessPoints] ADD [IsSecure] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210624083006_IsSecureAndIsHidden', N'3.1.16');

GO


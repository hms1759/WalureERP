IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [WalureRole] (
    [Id] uniqueidentifier NOT NULL,
    [InBuilt] bit NOT NULL,
    [CreatedOn] datetime2 NULL,
    [ModifiedOn] datetime2 NULL,
    [CreatedBy] uniqueidentifier NULL,
    [ModifiedBy] uniqueidentifier NULL,
    [Name] nvarchar(max) NULL,
    [NormalizedName] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_WalureRole] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [WalureRoleClaim] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_WalureRoleClaim] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [WalureUser] (
    [Id] uniqueidentifier NOT NULL,
    [LastName] nvarchar(max) NULL,
    [FirstName] nvarchar(max) NULL,
    [MiddleName] nvarchar(max) NULL,
    [RefreshToken] nvarchar(max) NULL,
    [UserType] int NOT NULL,
    [Gender] int NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [ModifiedBy] nvarchar(max) NULL,
    [CreatedOn] datetime2 NOT NULL,
    [ModifiedOn] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [UserName] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_WalureUser] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [WalureUserClaim] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_WalureUserClaim] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [WalureUserLogin] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_WalureUserLogin] PRIMARY KEY ([LoginProvider], [ProviderKey])
);
GO

CREATE TABLE [WalureUserRole] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_WalureUserRole] PRIMARY KEY ([UserId], [RoleId])
);
GO

CREATE TABLE [WalureUserToken] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_WalureUserToken] PRIMARY KEY ([UserId])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'InBuilt', N'ModifiedBy', N'ModifiedOn', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[WalureRole]'))
    SET IDENTITY_INSERT [WalureRole] ON;
INSERT INTO [WalureRole] ([Id], [ConcurrencyStamp], [CreatedBy], [CreatedOn], [InBuilt], [ModifiedBy], [ModifiedOn], [Name], [NormalizedName])
VALUES ('3134fd36-f284-4634-a2d1-31f6ddef2668', N'42b13c0386ce4c9baaea63aec3ade775', NULL, '2019-09-16T00:00:00.0000000', CAST(1 AS bit), NULL, '2019-09-16T00:00:00.0000000', N'WALURE_BASIC_USER', N'WALURE_BASIC_USER'),
('3134ff36-f284-4634-a2d1-31f6ddaf2668', N'42b12c0386ce4c9baaea63aec3ade765', NULL, '2019-09-16T00:00:00.0000000', CAST(1 AS bit), NULL, '2019-09-16T00:00:00.0000000', N'WALURE_ADMIN', N'WALURE_ADMIN'),
('a1b6b6b0-0825-4975-a93d-df3dc86f8cc7', N'e437a567e45c4b01a5d1cefe125023d7', NULL, '2019-09-16T00:00:00.0000000', CAST(1 AS bit), NULL, '2019-09-16T00:00:00.0000000', N'INBUILT_ADMIN', N'INBUILT_ADMIN');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'InBuilt', N'ModifiedBy', N'ModifiedOn', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[WalureRole]'))
    SET IDENTITY_INSERT [WalureRole] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'RoleId') AND [object_id] = OBJECT_ID(N'[WalureRoleClaim]'))
    SET IDENTITY_INSERT [WalureRoleClaim] ON;
INSERT INTO [WalureRoleClaim] ([Id], [ClaimType], [ClaimValue], [RoleId])
VALUES (4, N'WalureRoleClaim', N'FULL_CONTROL', 'a1b6b6b0-0825-4975-a93d-df3dc86f8cc7'),
(5, N'WalureRoleClaim', N'FULL_DEPARTMENT_CONTROL', '3134ff36-f284-4634-a2d1-31f6ddaf2668'),
(6, N'WalureRoleClaim', N'FULL_OFFICE_CONTROL', '3134fd36-f284-4634-a2d1-31f6ddef2668');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'RoleId') AND [object_id] = OBJECT_ID(N'[WalureRoleClaim]'))
    SET IDENTITY_INSERT [WalureRoleClaim] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'Email', N'EmailConfirmed', N'FirstName', N'Gender', N'IsDeleted', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'MiddleName', N'ModifiedBy', N'ModifiedOn', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'RefreshToken', N'SecurityStamp', N'TwoFactorEnabled', N'UserName', N'UserType') AND [object_id] = OBJECT_ID(N'[WalureUser]'))
    SET IDENTITY_INSERT [WalureUser] ON;
INSERT INTO [WalureUser] ([Id], [AccessFailedCount], [ConcurrencyStamp], [CreatedBy], [CreatedOn], [Email], [EmailConfirmed], [FirstName], [Gender], [IsDeleted], [LastName], [LockoutEnabled], [LockoutEnd], [MiddleName], [ModifiedBy], [ModifiedOn], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [RefreshToken], [SecurityStamp], [TwoFactorEnabled], [UserName], [UserType])
VALUES ('e576df51-cec4-44a2-b4c2-ebf25178ea8f', 0, N'9c9bc80e-868d-440a-aa0c-b7b6bfb020cc', N'Admin@walurecapital.com', '2019-09-16T00:00:00.0000000', N'Admin@walurecapital.com', CAST(1 AS bit), NULL, 0, CAST(0 AS bit), NULL, CAST(0 AS bit), NULL, NULL, N'Admin@walurecapital.com', '2019-09-16T00:00:00.0000000', N'ADMIN@WALURECAPITAL.COM', N'ADMIN@WALURECAPITAL.COM', N'$2a$11$NPYab1i6VFlxmbE59MNgt.vzW1CY3XSX9Va4eNIEaGIFvPO3qeXpq', N'08009300832', CAST(1 AS bit), NULL, N'536f8ac3-0df8-45d2-8f34-630d0a2ed6e6', CAST(0 AS bit), N'Admin@walurecapital.com', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'Email', N'EmailConfirmed', N'FirstName', N'Gender', N'IsDeleted', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'MiddleName', N'ModifiedBy', N'ModifiedOn', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'RefreshToken', N'SecurityStamp', N'TwoFactorEnabled', N'UserName', N'UserType') AND [object_id] = OBJECT_ID(N'[WalureUser]'))
    SET IDENTITY_INSERT [WalureUser] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[WalureUserRole]'))
    SET IDENTITY_INSERT [WalureUserRole] ON;
INSERT INTO [WalureUserRole] ([RoleId], [UserId])
VALUES ('a1b6b6b0-0825-4975-a93d-df3dc86f8cc7', '9bf9c4ed-96dd-40b2-a63d-b0aed45f2848');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[WalureUserRole]'))
    SET IDENTITY_INSERT [WalureUserRole] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221118124023_init', N'7.0.0');
GO

COMMIT;
GO


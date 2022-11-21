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

CREATE TABLE [OpenIddictApplications] (
    [Id] uniqueidentifier NOT NULL,
    [AppId] nvarchar(max) NULL,
    [Language] nvarchar(max) NULL,
    [ClientId] nvarchar(100) NULL,
    [ClientSecret] nvarchar(max) NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [ConsentType] nvarchar(50) NULL,
    [DisplayName] nvarchar(max) NULL,
    [DisplayNames] nvarchar(max) NULL,
    [Permissions] nvarchar(max) NULL,
    [PostLogoutRedirectUris] nvarchar(max) NULL,
    [Properties] nvarchar(max) NULL,
    [RedirectUris] nvarchar(max) NULL,
    [Requirements] nvarchar(max) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictApplications] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [OpenIddictScopes] (
    [Id] uniqueidentifier NOT NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [Description] nvarchar(max) NULL,
    [Descriptions] nvarchar(max) NULL,
    [DisplayName] nvarchar(max) NULL,
    [DisplayNames] nvarchar(max) NULL,
    [Name] nvarchar(200) NULL,
    [Properties] nvarchar(max) NULL,
    [Resources] nvarchar(max) NULL,
    CONSTRAINT [PK_OpenIddictScopes] PRIMARY KEY ([Id])
);
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

CREATE TABLE [OpenIddictAuthorizations] (
    [Id] uniqueidentifier NOT NULL,
    [ApplicationId] uniqueidentifier NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [CreationDate] datetime2 NULL,
    [Properties] nvarchar(max) NULL,
    [Scopes] nvarchar(max) NULL,
    [Status] nvarchar(50) NULL,
    [Subject] nvarchar(400) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictAuthorizations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [OpenIddictApplications] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [OpenIddictTokens] (
    [Id] uniqueidentifier NOT NULL,
    [ApplicationId] uniqueidentifier NULL,
    [AuthorizationId] uniqueidentifier NULL,
    [ConcurrencyToken] nvarchar(50) NULL,
    [CreationDate] datetime2 NULL,
    [ExpirationDate] datetime2 NULL,
    [Payload] nvarchar(max) NULL,
    [Properties] nvarchar(max) NULL,
    [RedemptionDate] datetime2 NULL,
    [ReferenceId] nvarchar(100) NULL,
    [Status] nvarchar(50) NULL,
    [Subject] nvarchar(400) NULL,
    [Type] nvarchar(50) NULL,
    CONSTRAINT [PK_OpenIddictTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OpenIddictTokens_OpenIddictApplications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [OpenIddictApplications] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId] FOREIGN KEY ([AuthorizationId]) REFERENCES [OpenIddictAuthorizations] ([Id]) ON DELETE NO ACTION
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'InBuilt', N'ModifiedBy', N'ModifiedOn', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[WalureRole]'))
    SET IDENTITY_INSERT [WalureRole] ON;
INSERT INTO [WalureRole] ([Id], [ConcurrencyStamp], [CreatedBy], [CreatedOn], [InBuilt], [ModifiedBy], [ModifiedOn], [Name], [NormalizedName])
VALUES ('57240ef7-daba-4ce2-9224-34f5ea110f55', N'499189c0-6778-4b71-9780-682329d11b64', NULL, '2019-09-16T00:00:00.0000000', CAST(0 AS bit), NULL, '2019-09-16T00:00:00.0000000', N'WALURE_ADMIN', N'WALURE_ADMIN'),
('5a30ac88-4cce-45e5-95c3-4f540b682402', N'8882151e-04b9-4c58-bddf-e6ed0be8f822', NULL, '2019-09-16T00:00:00.0000000', CAST(0 AS bit), NULL, '2019-09-16T00:00:00.0000000', N'WALURE_BASIC_USER', N'WALURE_BASIC_USER'),
('62ba02d3-fdc4-4a57-9db5-6608212f1106', N'04227ea6-16e8-4e33-b129-6fe6ace1483f', NULL, '2019-09-16T00:00:00.0000000', CAST(1 AS bit), NULL, '2019-09-16T00:00:00.0000000', N'INBUILT_ADMIN', N'INBUILT_ADMIN');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'InBuilt', N'ModifiedBy', N'ModifiedOn', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[WalureRole]'))
    SET IDENTITY_INSERT [WalureRole] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'RoleId') AND [object_id] = OBJECT_ID(N'[WalureRoleClaim]'))
    SET IDENTITY_INSERT [WalureRoleClaim] ON;
INSERT INTO [WalureRoleClaim] ([Id], [ClaimType], [ClaimValue], [RoleId])
VALUES (4, N'WalureRoleClaim', N'FULL_CONTROL', '62ba02d3-fdc4-4a57-9db5-6608212f1106'),
(5, N'WalureRoleClaim', N'FULL_DEPARTMENT_CONTROL', '57240ef7-daba-4ce2-9224-34f5ea110f55'),
(6, N'WalureRoleClaim', N'FULL_OFFICE_CONTROL', '5a30ac88-4cce-45e5-95c3-4f540b682402');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'RoleId') AND [object_id] = OBJECT_ID(N'[WalureRoleClaim]'))
    SET IDENTITY_INSERT [WalureRoleClaim] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'Email', N'EmailConfirmed', N'FirstName', N'Gender', N'IsDeleted', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'MiddleName', N'ModifiedBy', N'ModifiedOn', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'RefreshToken', N'SecurityStamp', N'TwoFactorEnabled', N'UserName', N'UserType') AND [object_id] = OBJECT_ID(N'[WalureUser]'))
    SET IDENTITY_INSERT [WalureUser] ON;
INSERT INTO [WalureUser] ([Id], [AccessFailedCount], [ConcurrencyStamp], [CreatedBy], [CreatedOn], [Email], [EmailConfirmed], [FirstName], [Gender], [IsDeleted], [LastName], [LockoutEnabled], [LockoutEnd], [MiddleName], [ModifiedBy], [ModifiedOn], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [RefreshToken], [SecurityStamp], [TwoFactorEnabled], [UserName], [UserType])
VALUES ('9bf9c4ed-96dd-40b2-a63d-b0aed45f2848', 1, N'7af8316c-c568-4aee-a9ed-dadf70bdd1fb', N'Admin@walurecapital.com', '2019-09-16T00:00:00.0000000', N'Admin@walurecapital.com', CAST(0 AS bit), N'Admin', 0, CAST(0 AS bit), N'Admin', CAST(0 AS bit), NULL, NULL, N'Admin@walurecapital.com', '2019-09-16T00:00:00.0000000', N'ADMIN@WALURECAPITAL.COM', N'ADMIN@WALURECAPITAL.COM', N'AQAAAAIAAYagAAAAEHLMz4dGQpnckByhSAB8AbXEfQX8ZcSqyoGvuS+GAV3v72agVSrQ9JIl8IScaW5vZg==', N'08009300832', CAST(0 AS bit), NULL, N'6befaa93-a0e0-4bdd-8b37-6a04efe75f62', CAST(0 AS bit), N'Admin@walurecapital.com', 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'CreatedBy', N'CreatedOn', N'Email', N'EmailConfirmed', N'FirstName', N'Gender', N'IsDeleted', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'MiddleName', N'ModifiedBy', N'ModifiedOn', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'RefreshToken', N'SecurityStamp', N'TwoFactorEnabled', N'UserName', N'UserType') AND [object_id] = OBJECT_ID(N'[WalureUser]'))
    SET IDENTITY_INSERT [WalureUser] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[WalureUserRole]'))
    SET IDENTITY_INSERT [WalureUserRole] ON;
INSERT INTO [WalureUserRole] ([RoleId], [UserId])
VALUES ('62ba02d3-fdc4-4a57-9db5-6608212f1106', '9bf9c4ed-96dd-40b2-a63d-b0aed45f2848');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[WalureUserRole]'))
    SET IDENTITY_INSERT [WalureUserRole] OFF;
GO

CREATE UNIQUE INDEX [IX_OpenIddictApplications_ClientId] ON [OpenIddictApplications] ([ClientId]) WHERE [ClientId] IS NOT NULL;
GO

CREATE INDEX [IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type] ON [OpenIddictAuthorizations] ([ApplicationId], [Status], [Subject], [Type]);
GO

CREATE UNIQUE INDEX [IX_OpenIddictScopes_Name] ON [OpenIddictScopes] ([Name]) WHERE [Name] IS NOT NULL;
GO

CREATE INDEX [IX_OpenIddictTokens_ApplicationId_Status_Subject_Type] ON [OpenIddictTokens] ([ApplicationId], [Status], [Subject], [Type]);
GO

CREATE INDEX [IX_OpenIddictTokens_AuthorizationId] ON [OpenIddictTokens] ([AuthorizationId]);
GO

CREATE UNIQUE INDEX [IX_OpenIddictTokens_ReferenceId] ON [OpenIddictTokens] ([ReferenceId]) WHERE [ReferenceId] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221121102921_int', N'7.0.0');
GO

COMMIT;
GO


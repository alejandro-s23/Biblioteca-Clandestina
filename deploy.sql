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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329000305_ReCreateDatabase'
)
BEGIN
    CREATE TABLE [Books] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [Author] nvarchar(50) NOT NULL,
        [Avaliable] bit NOT NULL,
        CONSTRAINT [PK_Books] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329000305_ReCreateDatabase'
)
BEGIN
    CREATE TABLE [Clients] (
        [Id] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [City] nvarchar(max) NOT NULL,
        [District] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Registration] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [IsApproved] bit NOT NULL,
        [IsAdmin] bit NOT NULL,
        CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329000305_ReCreateDatabase'
)
BEGIN
    CREATE TABLE [BookRents] (
        [Id] uniqueidentifier NOT NULL,
        [BookId] uniqueidentifier NOT NULL,
        [ClientId] uniqueidentifier NOT NULL,
        [RentDate] datetime2 NOT NULL,
        [ReturnDate] datetime2 NULL,
        CONSTRAINT [PK_BookRents] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BookRents_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BookRents_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329000305_ReCreateDatabase'
)
BEGIN
    CREATE UNIQUE INDEX [IX_BookRents_BookId] ON [BookRents] ([BookId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329000305_ReCreateDatabase'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_BookRents_ClientId] ON [BookRents] ([ClientId]) WHERE [ReturnDate] IS NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329000305_ReCreateDatabase'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260329000305_ReCreateDatabase', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'Registration');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Clients] ALTER COLUMN [Registration] nvarchar(10) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'Phone');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [Phone] nvarchar(11) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'Password');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [Password] nvarchar(50) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'LastName');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var3 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [LastName] nvarchar(70) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var4 nvarchar(max);
    SELECT @var4 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'FirstName');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var4 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [FirstName] nvarchar(50) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var5 nvarchar(max);
    SELECT @var5 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'Email');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var5 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [Email] nvarchar(50) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var6 nvarchar(max);
    SELECT @var6 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'District');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var6 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [District] nvarchar(60) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var7 nvarchar(max);
    SELECT @var7 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'City');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var7 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [City] nvarchar(60) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    DECLARE @var8 nvarchar(max);
    SELECT @var8 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'Address');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var8 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [Address] nvarchar(60) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    ALTER TABLE [Clients] ADD [AddressNumber] nvarchar(10) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    ALTER TABLE [Books] ADD [Added] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329170943_UserAdressNumber'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260329170943_UserAdressNumber', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329174221_UserMultipleRentsRegister'
)
BEGIN
    DROP INDEX [IX_BookRents_BookId] ON [BookRents];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329174221_UserMultipleRentsRegister'
)
BEGIN
    ALTER TABLE [Books] ADD [IdCurrentRent] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329174221_UserMultipleRentsRegister'
)
BEGIN
    CREATE INDEX [IX_Books_IdCurrentRent] ON [Books] ([IdCurrentRent]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329174221_UserMultipleRentsRegister'
)
BEGIN
    CREATE INDEX [IX_BookRents_BookId] ON [BookRents] ([BookId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329174221_UserMultipleRentsRegister'
)
BEGIN
    ALTER TABLE [Books] ADD CONSTRAINT [FK_Books_BookRents_IdCurrentRent] FOREIGN KEY ([IdCurrentRent]) REFERENCES [BookRents] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260329174221_UserMultipleRentsRegister'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260329174221_UserMultipleRentsRegister', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330143220_CPF'
)
BEGIN
    DECLARE @var9 nvarchar(max);
    SELECT @var9 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'Registration');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var9 + ';');
    ALTER TABLE [Clients] ALTER COLUMN [Registration] nvarchar(15) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330143220_CPF'
)
BEGIN
    ALTER TABLE [Clients] ADD [CPF] nvarchar(11) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330143220_CPF'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260330143220_CPF', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330182950_ComplementClient'
)
BEGIN
    ALTER TABLE [Clients] ADD [Complement] nvarchar(50) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260330182950_ComplementClient'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260330182950_ComplementClient', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205101_ClientToUserRefact'
)
BEGIN
    ALTER TABLE [BookRents] DROP CONSTRAINT [FK_BookRents_Clients_ClientId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205101_ClientToUserRefact'
)
BEGIN
    ALTER TABLE [BookRents] ADD [UserId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205101_ClientToUserRefact'
)
BEGIN
    CREATE INDEX [IX_BookRents_UserId] ON [BookRents] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205101_ClientToUserRefact'
)
BEGIN
    ALTER TABLE [BookRents] ADD CONSTRAINT [FK_BookRents_Clients_UserId] FOREIGN KEY ([UserId]) REFERENCES [Clients] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205101_ClientToUserRefact'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260404205101_ClientToUserRefact', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205717_ClientToUserRefacts'
)
BEGIN
    ALTER TABLE [BookRents] DROP CONSTRAINT [FK_BookRents_Clients_UserId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205717_ClientToUserRefacts'
)
BEGIN
    ALTER TABLE [Clients] DROP CONSTRAINT [PK_Clients];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205717_ClientToUserRefacts'
)
BEGIN
    EXEC sp_rename N'[Clients]', N'Users', 'OBJECT';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205717_ClientToUserRefacts'
)
BEGIN
    ALTER TABLE [Users] ADD CONSTRAINT [PK_Users] PRIMARY KEY ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205717_ClientToUserRefacts'
)
BEGIN
    ALTER TABLE [BookRents] ADD CONSTRAINT [FK_BookRents_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260404205717_ClientToUserRefacts'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260404205717_ClientToUserRefacts', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406141733_ChangeClientIdToUserId'
)
BEGIN
    ALTER TABLE [BookRents] DROP CONSTRAINT [FK_BookRents_Users_UserId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406141733_ChangeClientIdToUserId'
)
BEGIN
    DROP INDEX [IX_BookRents_ClientId] ON [BookRents];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406141733_ChangeClientIdToUserId'
)
BEGIN
    DROP INDEX [IX_BookRents_UserId] ON [BookRents];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406141733_ChangeClientIdToUserId'
)
BEGIN
    DECLARE @var10 nvarchar(max);
    SELECT @var10 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BookRents]') AND [c].[name] = N'ClientId');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [BookRents] DROP CONSTRAINT ' + @var10 + ';');
    ALTER TABLE [BookRents] DROP COLUMN [ClientId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406141733_ChangeClientIdToUserId'
)
BEGIN
    DECLARE @var11 nvarchar(max);
    SELECT @var11 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BookRents]') AND [c].[name] = N'UserId');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [BookRents] DROP CONSTRAINT ' + @var11 + ';');
    EXEC(N'UPDATE [BookRents] SET [UserId] = ''00000000-0000-0000-0000-000000000000'' WHERE [UserId] IS NULL');
    ALTER TABLE [BookRents] ALTER COLUMN [UserId] uniqueidentifier NOT NULL;
    ALTER TABLE [BookRents] ADD DEFAULT '00000000-0000-0000-0000-000000000000' FOR [UserId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406141733_ChangeClientIdToUserId'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_BookRents_UserId] ON [BookRents] ([UserId]) WHERE [ReturnDate] IS NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406141733_ChangeClientIdToUserId'
)
BEGIN
    ALTER TABLE [BookRents] ADD CONSTRAINT [FK_BookRents_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406141733_ChangeClientIdToUserId'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260406141733_ChangeClientIdToUserId', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406194116_AddedRequestTable'
)
BEGIN
    CREATE TABLE [Requests] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Type] int NOT NULL,
        [Status] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [Body] json NOT NULL,
        CONSTRAINT [PK_Requests] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Requests_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406194116_AddedRequestTable'
)
BEGIN
    CREATE INDEX [IX_Requests_UserId] ON [Requests] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260406194116_AddedRequestTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260406194116_AddedRequestTable', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413130839_AddedBookDescription'
)
BEGIN
    EXEC sp_rename N'[Books].[Avaliable]', N'Available', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413130839_AddedBookDescription'
)
BEGIN
    DECLARE @var12 nvarchar(max);
    SELECT @var12 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Requests]') AND [c].[name] = N'UpdatedAt');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Requests] DROP CONSTRAINT ' + @var12 + ';');
    ALTER TABLE [Requests] ALTER COLUMN [UpdatedAt] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413130839_AddedBookDescription'
)
BEGIN
    ALTER TABLE [Books] ADD [Description] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260413130839_AddedBookDescription'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260413130839_AddedBookDescription', N'10.0.5');
END;

COMMIT;
GO


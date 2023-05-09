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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211230223219_AddData2DB')
BEGIN
    CREATE TABLE [GATreeData] (
        [Data_int] int NOT NULL IDENTITY,
        [Data_guid] nvarchar(max) NULL,
        [Data_Friendlyname] nvarchar(max) NULL,
        [Data_notes] nvarchar(max) NULL,
        [Data_json] nvarchar(max) NULL,
        CONSTRAINT [PK_GATreeData] PRIMARY KEY ([Data_int])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211230223219_AddData2DB')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211230223219_AddData2DB', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220101213722_AddDate2Data')
BEGIN
    EXEC sp_rename N'[GATreeData].[Data_Friendlyname]', N'Data_friendlyname', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220101213722_AddDate2Data')
BEGIN
    ALTER TABLE [GATreeData] ADD [Data_date] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220101213722_AddDate2Data')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220101213722_AddDate2Data', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220102213708_AddDataAttributes')
BEGIN
    EXEC sp_rename N'[GATreeData].[Data_int]', N'Data_id', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220102213708_AddDataAttributes')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GATreeData]') AND [c].[name] = N'Data_guid');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [GATreeData] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [GATreeData] ALTER COLUMN [Data_guid] nvarchar(max) NOT NULL;
    ALTER TABLE [GATreeData] ADD DEFAULT N'' FOR [Data_guid];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220102213708_AddDataAttributes')
BEGIN
    ALTER TABLE [GATreeData] ADD [Data_header] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220102213708_AddDataAttributes')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220102213708_AddDataAttributes', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220102214839_AddDataAttributesV2')
BEGIN
    CREATE TABLE [GATreeDataAttributes] (
        [DataAttributes_Id] int NOT NULL IDENTITY,
        [DataAttributes_sn] int NOT NULL,
        [DataAttributes_name] nvarchar(max) NULL,
        [DataAttributes_type] nvarchar(max) NULL,
        [DataAttributes_attributesData] nvarchar(max) NULL,
        [DataAttributes_isClass] bit NOT NULL,
        [data_DataAttributes_data_ID] int NOT NULL,
        CONSTRAINT [PK_GATreeDataAttributes] PRIMARY KEY ([DataAttributes_Id]),
        CONSTRAINT [FK_GATreeDataAttributes_GATreeData_data_DataAttributes_data_ID] FOREIGN KEY ([data_DataAttributes_data_ID]) REFERENCES [GATreeData] ([Data_id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220102214839_AddDataAttributesV2')
BEGIN
    CREATE INDEX [IX_GATreeDataAttributes_data_DataAttributes_data_ID] ON [GATreeDataAttributes] ([data_DataAttributes_data_ID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220102214839_AddDataAttributesV2')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220102214839_AddDataAttributesV2', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220104215007_AddGuit2Attributes')
BEGIN
    ALTER TABLE [GATreeDataAttributes] ADD [Data_guid] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220104215007_AddGuit2Attributes')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220104215007_AddGuit2Attributes', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220104220203_AddURL2Data')
BEGIN
    ALTER TABLE [GATreeData] ADD [Data_URL] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220104220203_AddURL2Data')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220104220203_AddURL2Data', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106005710_AddTestData')
BEGIN
    CREATE TABLE [TestsData] (
        [Test_id] int NOT NULL IDENTITY,
        [Test_friendlyName] nvarchar(max) NULL,
        [Test_notes] nvarchar(max) NULL,
        [Test_guid] nvarchar(max) NULL,
        [Test_params] nvarchar(max) NULL,
        [TestData_dateStart] datetime2 NOT NULL,
        [TestData_dateEnd] datetime2 NOT NULL,
        [TestData_stage] nvarchar(max) NULL,
        [TestData_results] nvarchar(max) NULL,
        [TestData_trees] nvarchar(max) NULL,
        [TestData_data_id] int NOT NULL,
        CONSTRAINT [PK_TestsData] PRIMARY KEY ([Test_id]),
        CONSTRAINT [FK_TestsData_GATreeData_TestData_data_id] FOREIGN KEY ([TestData_data_id]) REFERENCES [GATreeData] ([Data_id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106005710_AddTestData')
BEGIN
    CREATE INDEX [IX_TestsData_TestData_data_id] ON [TestsData] ([TestData_data_id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106005710_AddTestData')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220106005710_AddTestData', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106010117_AddTestDataV2')
BEGIN
    ALTER TABLE [TestsData] DROP CONSTRAINT [FK_TestsData_GATreeData_TestData_data_id];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106010117_AddTestDataV2')
BEGIN
    ALTER TABLE [TestsData] DROP CONSTRAINT [PK_TestsData];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106010117_AddTestDataV2')
BEGIN
    EXEC sp_rename N'[TestsData]', N'GATreeTestsData';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106010117_AddTestDataV2')
BEGIN
    EXEC sp_rename N'[GATreeTestsData].[IX_TestsData_TestData_data_id]', N'IX_GATreeTestsData_TestData_data_id', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106010117_AddTestDataV2')
BEGIN
    ALTER TABLE [GATreeTestsData] ADD CONSTRAINT [PK_GATreeTestsData] PRIMARY KEY ([Test_id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106010117_AddTestDataV2')
BEGIN
    ALTER TABLE [GATreeTestsData] ADD CONSTRAINT [FK_GATreeTestsData_GATreeData_TestData_data_id] FOREIGN KEY ([TestData_data_id]) REFERENCES [GATreeData] ([Data_id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106010117_AddTestDataV2')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220106010117_AddTestDataV2', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106011451_AddTestDataV3')
BEGIN
    EXEC sp_rename N'[GATreeTestsData].[Test_params]', N'TestData_params', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106011451_AddTestDataV3')
BEGIN
    EXEC sp_rename N'[GATreeTestsData].[Test_notes]', N'TestData_notes', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106011451_AddTestDataV3')
BEGIN
    EXEC sp_rename N'[GATreeTestsData].[Test_guid]', N'TestData_guid', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106011451_AddTestDataV3')
BEGIN
    EXEC sp_rename N'[GATreeTestsData].[Test_friendlyName]', N'TestData_friendlyName', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106011451_AddTestDataV3')
BEGIN
    EXEC sp_rename N'[GATreeTestsData].[Test_id]', N'TestData_id', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106011451_AddTestDataV3')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220106011451_AddTestDataV3', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106210300_AddRequiredFieldsInDataAndtestsData')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GATreeTestsData]') AND [c].[name] = N'TestData_friendlyName');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [GATreeTestsData] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [GATreeTestsData] ALTER COLUMN [TestData_friendlyName] nvarchar(max) NOT NULL;
    ALTER TABLE [GATreeTestsData] ADD DEFAULT N'' FOR [TestData_friendlyName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106210300_AddRequiredFieldsInDataAndtestsData')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GATreeData]') AND [c].[name] = N'Data_friendlyname');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [GATreeData] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [GATreeData] ALTER COLUMN [Data_friendlyname] nvarchar(max) NOT NULL;
    ALTER TABLE [GATreeData] ADD DEFAULT N'' FOR [Data_friendlyname];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220106210300_AddRequiredFieldsInDataAndtestsData')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220106210300_AddRequiredFieldsInDataAndtestsData', N'5.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220108185100_AddMoreFieldsToDataAttributes')
BEGIN
    ALTER TABLE [GATreeTestsData] ADD [TestData_correctionfactor] real NOT NULL DEFAULT CAST(0 AS real);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220108185100_AddMoreFieldsToDataAttributes')
BEGIN
    ALTER TABLE [GATreeTestsData] ADD [TestData_crossoverPropability] real NOT NULL DEFAULT CAST(0 AS real);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220108185100_AddMoreFieldsToDataAttributes')
BEGIN
    ALTER TABLE [GATreeTestsData] ADD [TestData_generationGap] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220108185100_AddMoreFieldsToDataAttributes')
BEGIN
    ALTER TABLE [GATreeTestsData] ADD [TestData_mutationPropability] real NOT NULL DEFAULT CAST(0 AS real);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220108185100_AddMoreFieldsToDataAttributes')
BEGIN
    ALTER TABLE [GATreeTestsData] ADD [TestData_numberOfMaxGenerations] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220108185100_AddMoreFieldsToDataAttributes')
BEGIN
    ALTER TABLE [GATreeTestsData] ADD [TestData_numberOfMaxTrees] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220108185100_AddMoreFieldsToDataAttributes')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220108185100_AddMoreFieldsToDataAttributes', N'5.0.13');
END;
GO

COMMIT;
GO


CREATE TABLE [GATreeData] (
    [Data_id] int NOT NULL IDENTITY,
    [Data_guid] nvarchar(max) NOT NULL,
    [Data_friendlyname] nvarchar(max) NOT NULL,
    [Data_notes] nvarchar(max) NULL,
    [Data_json] nvarchar(max) NULL,
    [Data_date] datetime2 NOT NULL,
    [Data_header] nvarchar(max) NULL,
    [Data_URL] nvarchar(max) NULL,
    CONSTRAINT [PK_GATreeData] PRIMARY KEY ([Data_id])
);
GO


CREATE TABLE [GATreeDataAttributes] (
    [DataAttributes_Id] int NOT NULL IDENTITY,
    [DataAttributes_sn] int NOT NULL,
    [DataAttributes_name] nvarchar(max) NULL,
    [DataAttributes_type] nvarchar(max) NULL,
    [DataAttributes_attributesData] nvarchar(max) NULL,
    [DataAttributes_isClass] bit NOT NULL,
    [data_DataAttributes_data_ID] int NOT NULL,
    [Data_guid] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GATreeDataAttributes] PRIMARY KEY ([DataAttributes_Id]),
    CONSTRAINT [FK_GATreeDataAttributes_GATreeData_data_DataAttributes_data_ID] FOREIGN KEY ([data_DataAttributes_data_ID]) REFERENCES [GATreeData] ([Data_id]) ON DELETE CASCADE
);
GO


CREATE TABLE [GATreeTestsData] (
    [TestData_id] int NOT NULL IDENTITY,
    [TestData_friendlyName] nvarchar(max) NOT NULL,
    [TestData_notes] nvarchar(max) NULL,
    [TestData_guid] nvarchar(max) NULL,
    [TestData_numberOfMaxTrees] int NOT NULL,
    [TestData_numberOfMaxGenerations] int NOT NULL,
    [TestData_mutationPropability] real NOT NULL,
    [TestData_crossoverPropability] real NOT NULL,
    [TestData_generationGap] int NOT NULL,
    [TestData_correctionfactor] real NOT NULL,
    [TestData_params] nvarchar(max) NULL,
    [TestData_dateStart] datetime2 NOT NULL,
    [TestData_dateEnd] datetime2 NOT NULL,
    [TestData_stage] nvarchar(max) NULL,
    [TestData_results] nvarchar(max) NULL,
    [TestData_trees] nvarchar(max) NULL,
    [TestData_data_id] int NOT NULL,
    CONSTRAINT [PK_GATreeTestsData] PRIMARY KEY ([TestData_id]),
    CONSTRAINT [FK_GATreeTestsData_GATreeData_TestData_data_id] FOREIGN KEY ([TestData_data_id]) REFERENCES [GATreeData] ([Data_id]) ON DELETE CASCADE
);
GO


CREATE INDEX [IX_GATreeDataAttributes_data_DataAttributes_data_ID] ON [GATreeDataAttributes] ([data_DataAttributes_data_ID]);
GO


CREATE INDEX [IX_GATreeTestsData_TestData_data_id] ON [GATreeTestsData] ([TestData_data_id]);
GO



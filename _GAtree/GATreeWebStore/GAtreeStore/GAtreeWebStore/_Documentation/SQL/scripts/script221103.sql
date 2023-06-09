USE [Test1]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/11/2022 11:44:19 πμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GATreeData]    Script Date: 3/11/2022 11:44:19 πμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GATreeData](
	[Data_id] [int] IDENTITY(1,1) NOT NULL,
	[Data_guid] [nvarchar](max) NOT NULL,
	[Data_friendlyname] [nvarchar](max) NOT NULL,
	[Data_notes] [nvarchar](max) NULL,
	[Data_json] [nvarchar](max) NULL,
	[Data_date] [datetime2](7) NOT NULL,
	[Data_header] [nvarchar](max) NULL,
	[Data_URL] [nvarchar](max) NULL,
 CONSTRAINT [PK_GATreeData] PRIMARY KEY CLUSTERED 
(
	[Data_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GATreeDataAttributes]    Script Date: 3/11/2022 11:44:19 πμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GATreeDataAttributes](
	[DataAttributes_Id] [int] IDENTITY(1,1) NOT NULL,
	[DataAttributes_sn] [int] NOT NULL,
	[DataAttributes_name] [nvarchar](max) NULL,
	[DataAttributes_type] [nvarchar](max) NULL,
	[DataAttributes_attributesData] [nvarchar](max) NULL,
	[DataAttributes_isClass] [bit] NOT NULL,
	[data_DataAttributes_data_ID] [int] NOT NULL,
	[DataAttributes_guid] [nvarchar](max) NOT NULL,
	[data_DataAttributes_intMax] [int] NOT NULL,
	[data_DataAttributes_floatMax] [real] NOT NULL,
	[data_DataAttributes_floatMin] [real] NOT NULL,
	[data_DataAttributes_intMin] [int] NOT NULL,
	[DataAttributes_hasNullValues] [bit] NOT NULL,
 CONSTRAINT [PK_GATreeDataAttributes] PRIMARY KEY CLUSTERED 
(
	[DataAttributes_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GATreeLogging]    Script Date: 3/11/2022 11:44:19 πμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GATreeLogging](
	[Error_id] [int] IDENTITY(1,1) NOT NULL,
	[Data_date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_GATreeLogging] PRIMARY KEY CLUSTERED 
(
	[Error_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GATreeTestDataRun]    Script Date: 3/11/2022 11:44:19 πμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GATreeTestDataRun](
	[Test_data_run_id] [int] IDENTITY(1,1) NOT NULL,
	[Test_data_run_guid] [nvarchar](max) NOT NULL,
	[Test_data_run_dateStart] [datetime2](7) NOT NULL,
	[Test_data_run_dateEnd] [datetime2](7) NOT NULL,
	[TestData_id] [int] NOT NULL,
 CONSTRAINT [PK_GATreeTestDataRun] PRIMARY KEY CLUSTERED 
(
	[Test_data_run_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GATreeTestDataRunLog]    Script Date: 3/11/2022 11:44:19 πμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GATreeTestDataRunLog](
	[Test_data_run_log_id] [int] IDENTITY(1,1) NOT NULL,
	[Test_data_run_log_guid] [nvarchar](max) NOT NULL,
	[Test_data_run_log_StepNumber] [int] NOT NULL,
	[Test_data_run_log_data] [nvarchar](max) NULL,
	[Test_data_run_log_BestResult] [nvarchar](max) NULL,
	[Test_data_run_id] [int] NULL,
 CONSTRAINT [PK_GATreeTestDataRunLog] PRIMARY KEY CLUSTERED 
(
	[Test_data_run_log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GATreeTestsData]    Script Date: 3/11/2022 11:44:19 πμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GATreeTestsData](
	[TestData_id] [int] IDENTITY(1,1) NOT NULL,
	[TestData_friendlyName] [nvarchar](max) NOT NULL,
	[TestData_notes] [nvarchar](max) NULL,
	[TestData_guid] [nvarchar](max) NOT NULL,
	[TestData_params] [nvarchar](max) NULL,
	[TestData_data_id] [int] NOT NULL,
	[TestData_correctionfactor] [real] NOT NULL,
	[TestData_crossoverPropability] [real] NOT NULL,
	[TestData_generationGap] [int] NOT NULL,
	[TestData_mutationPropability] [real] NOT NULL,
	[TestData_numberOfMaxGenerations] [int] NOT NULL,
	[TestData_numberOfMaxTrees] [int] NOT NULL,
 CONSTRAINT [PK_GATreeTestsData] PRIMARY KEY CLUSTERED 
(
	[TestData_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GATreeThinClient]    Script Date: 3/11/2022 11:44:19 πμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GATreeThinClient](
	[ThinClient_id] [int] IDENTITY(1,1) NOT NULL,
	[ThinClient_guid] [nvarchar](max) NOT NULL,
	[Test_data_run_id] [int] NOT NULL,
	[testdatarunTest_data_run_id] [int] NULL,
	[ThinClient_isRunActive] [bit] NOT NULL,
	[thinClient_isActive] [bit] NOT NULL,
 CONSTRAINT [PK_GATreeThinClient] PRIMARY KEY CLUSTERED 
(
	[ThinClient_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[GATreeData] ADD  DEFAULT (N'') FOR [Data_guid]
GO
ALTER TABLE [dbo].[GATreeData] ADD  DEFAULT (N'') FOR [Data_friendlyname]
GO
ALTER TABLE [dbo].[GATreeData] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Data_date]
GO
ALTER TABLE [dbo].[GATreeDataAttributes] ADD  DEFAULT (N'') FOR [DataAttributes_guid]
GO
ALTER TABLE [dbo].[GATreeDataAttributes] ADD  DEFAULT ((0)) FOR [data_DataAttributes_intMax]
GO
ALTER TABLE [dbo].[GATreeDataAttributes] ADD  DEFAULT (CONVERT([real],(0))) FOR [data_DataAttributes_floatMax]
GO
ALTER TABLE [dbo].[GATreeDataAttributes] ADD  DEFAULT (CONVERT([real],(0))) FOR [data_DataAttributes_floatMin]
GO
ALTER TABLE [dbo].[GATreeDataAttributes] ADD  DEFAULT ((0)) FOR [data_DataAttributes_intMin]
GO
ALTER TABLE [dbo].[GATreeDataAttributes] ADD  DEFAULT (CONVERT([bit],(0))) FOR [DataAttributes_hasNullValues]
GO
ALTER TABLE [dbo].[GATreeTestDataRun] ADD  DEFAULT ((0)) FOR [TestData_id]
GO
ALTER TABLE [dbo].[GATreeTestsData] ADD  DEFAULT (N'') FOR [TestData_friendlyName]
GO
ALTER TABLE [dbo].[GATreeTestsData] ADD  DEFAULT (N'') FOR [TestData_guid]
GO
ALTER TABLE [dbo].[GATreeTestsData] ADD  DEFAULT (CONVERT([real],(0))) FOR [TestData_correctionfactor]
GO
ALTER TABLE [dbo].[GATreeTestsData] ADD  DEFAULT (CONVERT([real],(0))) FOR [TestData_crossoverPropability]
GO
ALTER TABLE [dbo].[GATreeTestsData] ADD  DEFAULT ((0)) FOR [TestData_generationGap]
GO
ALTER TABLE [dbo].[GATreeTestsData] ADD  DEFAULT (CONVERT([real],(0))) FOR [TestData_mutationPropability]
GO
ALTER TABLE [dbo].[GATreeTestsData] ADD  DEFAULT ((0)) FOR [TestData_numberOfMaxGenerations]
GO
ALTER TABLE [dbo].[GATreeTestsData] ADD  DEFAULT ((0)) FOR [TestData_numberOfMaxTrees]
GO
ALTER TABLE [dbo].[GATreeThinClient] ADD  DEFAULT (CONVERT([bit],(0))) FOR [ThinClient_isRunActive]
GO
ALTER TABLE [dbo].[GATreeThinClient] ADD  DEFAULT (CONVERT([bit],(0))) FOR [thinClient_isActive]
GO
ALTER TABLE [dbo].[GATreeDataAttributes]  WITH CHECK ADD  CONSTRAINT [FK_GATreeDataAttributes_GATreeData_data_DataAttributes_data_ID] FOREIGN KEY([data_DataAttributes_data_ID])
REFERENCES [dbo].[GATreeData] ([Data_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GATreeDataAttributes] CHECK CONSTRAINT [FK_GATreeDataAttributes_GATreeData_data_DataAttributes_data_ID]
GO
ALTER TABLE [dbo].[GATreeTestDataRun]  WITH CHECK ADD  CONSTRAINT [FK_GATreeTestDataRun_GATreeTestsData_TestData_id] FOREIGN KEY([TestData_id])
REFERENCES [dbo].[GATreeTestsData] ([TestData_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GATreeTestDataRun] CHECK CONSTRAINT [FK_GATreeTestDataRun_GATreeTestsData_TestData_id]
GO
ALTER TABLE [dbo].[GATreeTestDataRunLog]  WITH CHECK ADD  CONSTRAINT [FK_GATreeTestDataRunLog_GATreeTestDataRun_Test_data_run_id] FOREIGN KEY([Test_data_run_id])
REFERENCES [dbo].[GATreeTestDataRun] ([Test_data_run_id])
GO
ALTER TABLE [dbo].[GATreeTestDataRunLog] CHECK CONSTRAINT [FK_GATreeTestDataRunLog_GATreeTestDataRun_Test_data_run_id]
GO
ALTER TABLE [dbo].[GATreeTestsData]  WITH CHECK ADD  CONSTRAINT [FK_GATreeTestsData_GATreeData_TestData_data_id] FOREIGN KEY([TestData_data_id])
REFERENCES [dbo].[GATreeData] ([Data_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GATreeTestsData] CHECK CONSTRAINT [FK_GATreeTestsData_GATreeData_TestData_data_id]
GO
ALTER TABLE [dbo].[GATreeThinClient]  WITH CHECK ADD  CONSTRAINT [FK_GATreeThinClient_GATreeTestDataRun_testdatarunTest_data_run_id] FOREIGN KEY([testdatarunTest_data_run_id])
REFERENCES [dbo].[GATreeTestDataRun] ([Test_data_run_id])
GO
ALTER TABLE [dbo].[GATreeThinClient] CHECK CONSTRAINT [FK_GATreeThinClient_GATreeTestDataRun_testdatarunTest_data_run_id]
GO

﻿// <auto-generated />
using System;
using GAtreeWebStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GAtreeWebStore.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20221121105337_RenameTestDataRunLogFieldsAndTestDataRunFields")]
    partial class RenameTestDataRunLogFieldsAndTestDataRunFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GAtreeWebStore.Models.Data", b =>
                {
                    b.Property<int>("Data_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data_URL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Data_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data_friendlyname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data_header")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data_json")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data_notes")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Data_id");

                    b.ToTable("GATreeData");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.DataAttributes", b =>
                {
                    b.Property<int>("DataAttributes_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DataAttributes_attributesData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataAttributes_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DataAttributes_hasNullValues")
                        .HasColumnType("bit");

                    b.Property<bool>("DataAttributes_isClass")
                        .HasColumnType("bit");

                    b.Property<string>("DataAttributes_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DataAttributes_sn")
                        .HasColumnType("int");

                    b.Property<string>("DataAttributes_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("data_DataAttributes_data_ID")
                        .HasColumnType("int");

                    b.Property<float>("data_DataAttributes_floatMax")
                        .HasColumnType("real");

                    b.Property<float>("data_DataAttributes_floatMin")
                        .HasColumnType("real");

                    b.Property<int>("data_DataAttributes_intMax")
                        .HasColumnType("int");

                    b.Property<int>("data_DataAttributes_intMin")
                        .HasColumnType("int");

                    b.HasKey("DataAttributes_Id");

                    b.HasIndex("data_DataAttributes_data_ID");

                    b.ToTable("GATreeDataAttributes");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.Logging", b =>
                {
                    b.Property<int>("Error_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Data_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Error_id");

                    b.ToTable("GATreeLogging");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.RecordTree", b =>
                {
                    b.Property<int>("RecordOfTrees_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IterationNo")
                        .HasColumnType("int");

                    b.Property<string>("RecordOfTrees_data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecordOfTrees_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecordOfTrees_treeStatus")
                        .HasColumnType("int");

                    b.Property<int>("Test_data_run_id")
                        .HasColumnType("int");

                    b.Property<int>("ThinClient_id")
                        .HasColumnType("int");

                    b.Property<int>("cycleStage")
                        .HasColumnType("int");

                    b.Property<int>("runStage")
                        .HasColumnType("int");

                    b.HasKey("RecordOfTrees_id");

                    b.HasIndex("Test_data_run_id");

                    b.HasIndex("ThinClient_id");

                    b.ToTable("GATRecordOfTrees");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestData", b =>
                {
                    b.Property<int>("TestData_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Data_id")
                        .HasColumnType("int");

                    b.Property<float>("TestData_correctionfactor")
                        .HasColumnType("real");

                    b.Property<float>("TestData_crossoverPropability")
                        .HasColumnType("real");

                    b.Property<int>("TestData_data_id")
                        .HasColumnType("int");

                    b.Property<string>("TestData_friendlyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestData_generationGap")
                        .HasColumnType("int");

                    b.Property<string>("TestData_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("TestData_mutationPropability")
                        .HasColumnType("real");

                    b.Property<string>("TestData_notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestData_numberOfMaxGenerations")
                        .HasColumnType("int");

                    b.Property<int>("TestData_numberOfMaxTrees")
                        .HasColumnType("int");

                    b.Property<string>("TestData_params")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TestData_id");

                    b.HasIndex("Data_id");

                    b.ToTable("GATreeTestsData");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestDataRun", b =>
                {
                    b.Property<int>("Test_data_run_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TestDataRun_CurrentIterationNumber")
                        .HasColumnType("int");

                    b.Property<int>("TestDataRun_MaxIterationNumber")
                        .HasColumnType("int");

                    b.Property<int>("TestData_id")
                        .HasColumnType("int");

                    b.Property<int?>("TestData_id1")
                        .HasColumnType("int");

                    b.Property<int>("Test_data_cycleStage")
                        .HasColumnType("int");

                    b.Property<string>("Test_data_run_comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Test_data_run_dateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Test_data_run_dateStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("Test_data_run_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Test_data_run_results")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Test_data_run_runStage")
                        .HasColumnType("int");

                    b.HasKey("Test_data_run_id");

                    b.HasIndex("TestData_id1");

                    b.ToTable("GATreeTestDataRun");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestDataRunLog", b =>
                {
                    b.Property<int>("TestDataRunLog_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TestDataRunLog_BestResult")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestDataRunLog_IterationNo")
                        .HasColumnType("int");

                    b.Property<string>("TestDataRunLog_comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TestDataRunLog_data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TestDataRunLog_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Test_data_run_id")
                        .HasColumnType("int");

                    b.HasKey("TestDataRunLog_id");

                    b.HasIndex("Test_data_run_id");

                    b.ToTable("GATreeTestDataRunLog");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.ThinClient", b =>
                {
                    b.Property<int>("ThinClient_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Test_data_run_id")
                        .HasColumnType("int");

                    b.Property<string>("ThinClient_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ThinClient_isRunActive")
                        .HasColumnType("bit");

                    b.Property<int?>("testdatarunTest_data_run_id")
                        .HasColumnType("int");

                    b.Property<bool>("thinClient_isActive")
                        .HasColumnType("bit");

                    b.HasKey("ThinClient_id");

                    b.HasIndex("testdatarunTest_data_run_id");

                    b.ToTable("GATreeThinClient");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.DataAttributes", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.Data", "Data")
                        .WithMany()
                        .HasForeignKey("data_DataAttributes_data_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Data");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.RecordTree", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.TestDataRun", "testDataRun")
                        .WithMany()
                        .HasForeignKey("Test_data_run_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GAtreeWebStore.Models.ThinClient", "thinClient")
                        .WithMany()
                        .HasForeignKey("ThinClient_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("testDataRun");

                    b.Navigation("thinClient");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestData", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.Data", "Data")
                        .WithMany()
                        .HasForeignKey("Data_id");

                    b.Navigation("Data");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestDataRun", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.TestData", "testdata")
                        .WithMany()
                        .HasForeignKey("TestData_id1");

                    b.Navigation("testdata");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestDataRunLog", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.TestDataRun", "testdataRun")
                        .WithMany()
                        .HasForeignKey("Test_data_run_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("testdataRun");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.ThinClient", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.TestDataRun", "testdatarun")
                        .WithMany()
                        .HasForeignKey("testdatarunTest_data_run_id");

                    b.Navigation("testdatarun");
                });
#pragma warning restore 612, 618
        }
    }
}

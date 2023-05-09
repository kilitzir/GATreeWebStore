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
    [Migration("20221007080700_DataAttributesAddedMinMax")]
    partial class DataAttributesAddedMinMax
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

                    b.HasKey("DataAttributes_Id");

                    b.HasIndex("data_DataAttributes_data_ID");

                    b.ToTable("GATreeDataAttributes");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestData", b =>
                {
                    b.Property<int>("TestData_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.HasIndex("TestData_data_id");

                    b.ToTable("GATreeTestsData");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestDataRun", b =>
                {
                    b.Property<int>("Test_data_run_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("TestData_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Test_data_run_dateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Test_data_run_dateStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("Test_data_run_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Test_data_run_id");

                    b.HasIndex("TestData_id");

                    b.ToTable("GATreeTestDataRun");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestDataRunLog", b =>
                {
                    b.Property<int>("Test_data_run_log_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Test_data_run_id")
                        .HasColumnType("int");

                    b.Property<string>("Test_data_run_log_BestResult")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Test_data_run_log_StepNumber")
                        .HasColumnType("int");

                    b.Property<string>("Test_data_run_log_data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Test_data_run_log_guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Test_data_run_log_id");

                    b.HasIndex("Test_data_run_id");

                    b.ToTable("GATreeTestDataRunLog");
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

            modelBuilder.Entity("GAtreeWebStore.Models.TestData", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.Data", "Data")
                        .WithMany()
                        .HasForeignKey("TestData_data_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Data");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestDataRun", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.TestData", "testdata")
                        .WithMany()
                        .HasForeignKey("TestData_id");

                    b.Navigation("testdata");
                });

            modelBuilder.Entity("GAtreeWebStore.Models.TestDataRunLog", b =>
                {
                    b.HasOne("GAtreeWebStore.Models.TestData", "TestDataRun")
                        .WithMany()
                        .HasForeignKey("Test_data_run_id");

                    b.Navigation("TestDataRun");
                });
#pragma warning restore 612, 618
        }
    }
}

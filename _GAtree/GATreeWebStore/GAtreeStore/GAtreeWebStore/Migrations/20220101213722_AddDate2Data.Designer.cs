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
    [Migration("20220101213722_AddDate2Data")]
    partial class AddDate2Data
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
                    b.Property<int>("Data_int")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Data_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data_friendlyname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data_guid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data_json")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Data_notes")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Data_int");

                    b.ToTable("GATreeData");
                });
#pragma warning restore 612, 618
        }
    }
}
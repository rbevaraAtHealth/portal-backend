﻿// <auto-generated />
using System;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CodeMappingEfCore.Migrations
{
    [DbContext(typeof(CodeMatcherDbContext))]
    [Migration("20230727054252_RequestModelClientId")]
    partial class RequestModelClientId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeMappingDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Progress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("CodeMappings");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeMappingRequestDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CodeMappingId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LatestLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("RunSchedule")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RunTypeId")
                        .HasColumnType("int");

                    b.Property<int>("SegmentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Threshold")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CodeMappingId");

                    b.HasIndex("RunTypeId");

                    b.HasIndex("SegmentTypeId");

                    b.ToTable("CodeMappingRequests");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeMappingResponseDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<string>("ResponseMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("CodeMappingResponses");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.LookupDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("LookupTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LookupTypeId");

                    b.ToTable("Lookups");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.LookupTypeDto", b =>
                {
                    b.Property<int>("LookupTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LookupTypeId"), 1L, 1);

                    b.Property<string>("LookupTypeDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LookupTypeKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LookupTypeId");

                    b.ToTable("LookupTypes");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeMappingDto", b =>
                {
                    b.HasOne("CodeMappingEfCore.DatabaseModels.CodeMappingRequestDto", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Request");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeMappingRequestDto", b =>
                {
                    b.HasOne("CodeMappingEfCore.DatabaseModels.LookupDto", "CodeMappingType")
                        .WithMany()
                        .HasForeignKey("CodeMappingId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CodeMappingEfCore.DatabaseModels.LookupDto", "RunType")
                        .WithMany()
                        .HasForeignKey("RunTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CodeMappingEfCore.DatabaseModels.LookupDto", "SegmentType")
                        .WithMany()
                        .HasForeignKey("SegmentTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CodeMappingType");

                    b.Navigation("RunType");

                    b.Navigation("SegmentType");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeMappingResponseDto", b =>
                {
                    b.HasOne("CodeMappingEfCore.DatabaseModels.CodeMappingRequestDto", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Request");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.LookupDto", b =>
                {
                    b.HasOne("CodeMappingEfCore.DatabaseModels.LookupTypeDto", "LookupType")
                        .WithMany()
                        .HasForeignKey("LookupTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("LookupType");
                });
#pragma warning restore 612, 618
        }
    }
}

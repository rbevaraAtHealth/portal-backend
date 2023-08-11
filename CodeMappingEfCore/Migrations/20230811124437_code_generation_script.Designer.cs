﻿// <auto-generated />
using System;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CodeMatcher.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(CodeMatcherDbContext))]
    [Migration("20230811124437_code_generation_script")]
    partial class code_generation_script
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeGenerationOverwriteDto", b =>
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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Frm")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResponseReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Too")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CodeGenerationOverwrites");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeGenerationOverwriteHistoryDto", b =>
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

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("OverWriteID")
                        .HasColumnType("int");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OverWriteID");

                    b.ToTable("CodeGenerationOverwriteHistory");
                });

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

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LatestLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("RunSchedule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RunTypeId")
                        .HasColumnType("int");

                    b.Property<int>("SegmentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Threshold")
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

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
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

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.UserDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserDetail");
                });

            modelBuilder.Entity("CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables.CodeGenerationSummaryDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LatestLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("NoOfBaseRecords")
                        .HasColumnType("int");

                    b.Property<int>("NoOfInputRecords")
                        .HasColumnType("int");

                    b.Property<int>("NoOfProcessedRecords")
                        .HasColumnType("int");

                    b.Property<int>("NoOfRecordsForWhichCodeGenerated")
                        .HasColumnType("int");

                    b.Property<int>("NoOfRecordsForWhichCodeNotGenerated")
                        .HasColumnType("int");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<string>("Segment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StartLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Threshold")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("CodeGenerationSummary");
                });

            modelBuilder.Entity("CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables.MonthlyEmbeddingsSummaryDto", b =>
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

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("NoOfRecordsEmbeddingCreated")
                        .HasColumnType("int");

                    b.Property<int>("NoOfRecordsImportedFromDatabase")
                        .HasColumnType("int");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<string>("Segment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("MonthlyEmbeddingsSummary");
                });

            modelBuilder.Entity("CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables.WeeklyEmbeddingsSummaryDto", b =>
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

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LatestLink")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("NoOfBaseRecordsBeforeRun")
                        .HasColumnType("int");

                    b.Property<int>("NoOfBaseRecordsImportedFromDatabase")
                        .HasColumnType("int");

                    b.Property<int>("NoOfRecordsAfterRun")
                        .HasColumnType("int");

                    b.Property<int>("NoOfRecordsEmbeddingsCreated")
                        .HasColumnType("int");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<string>("Segment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StartLink")
                        .HasColumnType("int");

                    b.Property<string>("TaskId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("WeeklyEmbeddingsSummary");
                });

            modelBuilder.Entity("CodeMappingEfCore.DatabaseModels.CodeGenerationOverwriteHistoryDto", b =>
                {
                    b.HasOne("CodeMappingEfCore.DatabaseModels.CodeGenerationOverwriteDto", "CodeGenerationOverwrite")
                        .WithMany()
                        .HasForeignKey("OverWriteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CodeGenerationOverwrite");
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

            modelBuilder.Entity("CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables.CodeGenerationSummaryDto", b =>
                {
                    b.HasOne("CodeMappingEfCore.DatabaseModels.CodeMappingRequestDto", "CodeMappingRequest")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CodeMappingRequest");
                });

            modelBuilder.Entity("CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables.MonthlyEmbeddingsSummaryDto", b =>
                {
                    b.HasOne("CodeMappingEfCore.DatabaseModels.CodeMappingRequestDto", "CodeMappingRequest")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CodeMappingRequest");
                });

            modelBuilder.Entity("CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables.WeeklyEmbeddingsSummaryDto", b =>
                {
                    b.HasOne("CodeMappingEfCore.DatabaseModels.CodeMappingRequestDto", "CodeMappingRequest")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CodeMappingRequest");
                });
#pragma warning restore 612, 618
        }
    }
}

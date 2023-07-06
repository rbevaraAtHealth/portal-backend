﻿// <auto-generated />
using CodeMatcherV2Api.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeMatcherV2Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodeMatcherV2Api.Dtos.LookupDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LookupTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LookupTypeId");

                    b.ToTable("Lookups");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            LookupTypeId = 1,
                            Name = "School"
                        },
                        new
                        {
                            Id = 2,
                            LookupTypeId = 1,
                            Name = "Insurance"
                        },
                        new
                        {
                            Id = 3,
                            LookupTypeId = 1,
                            Name = "State License"
                        },
                        new
                        {
                            Id = 4,
                            LookupTypeId = 1,
                            Name = "Hospital"
                        });
                });

            modelBuilder.Entity("CodeMatcherV2Api.Dtos.LookupTypeDto", b =>
                {
                    b.Property<int>("LookupTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LookupTypeDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LookupTypeKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LookupTypeId");

                    b.ToTable("LookupTypes");

                    b.HasData(
                        new
                        {
                            LookupTypeId = 1,
                            LookupTypeDescription = "Segment defers jobs based on organization types.",
                            LookupTypeKey = "Segment"
                        });
                });

            modelBuilder.Entity("CodeMatcherV2Api.Dtos.LookupDto", b =>
                {
                    b.HasOne("CodeMatcherV2Api.Dtos.LookupTypeDto", "LookupTypeDto")
                        .WithMany()
                        .HasForeignKey("LookupTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMatcherV2Api.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LookupTypes",
                columns: table => new
                {
                    LookupTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LookupTypeKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LookupTypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupTypes", x => x.LookupTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Lookups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LookupTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lookups_LookupTypes_LookupTypeId",
                        column: x => x.LookupTypeId,
                        principalTable: "LookupTypes",
                        principalColumn: "LookupTypeId");
                });

            migrationBuilder.CreateTable(
                name: "CodeGenerations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunTypeId = table.Column<int>(type: "int", nullable: false),
                    SegmentTypeId = table.Column<int>(type: "int", nullable: false),
                    RunSchedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Threshold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatestLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGenerations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGenerations_Lookups_RunTypeId",
                        column: x => x.RunTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CodeGenerations_Lookups_SegmentTypeId",
                        column: x => x.SegmentTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Embeddings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunTypeId = table.Column<int>(type: "int", nullable: false),
                    SegmentTypeId = table.Column<int>(type: "int", nullable: false),
                    EmbeddingFrequencyId = table.Column<int>(type: "int", nullable: false),
                    RunSchedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Embeddings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Embeddings_Lookups_EmbeddingFrequencyId",
                        column: x => x.EmbeddingFrequencyId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Embeddings_Lookups_RunTypeId",
                        column: x => x.RunTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Embeddings_Lookups_SegmentTypeId",
                        column: x => x.SegmentTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeGenerations_RunTypeId",
                table: "CodeGenerations",
                column: "RunTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGenerations_SegmentTypeId",
                table: "CodeGenerations",
                column: "SegmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Embeddings_EmbeddingFrequencyId",
                table: "Embeddings",
                column: "EmbeddingFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Embeddings_RunTypeId",
                table: "Embeddings",
                column: "RunTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Embeddings_SegmentTypeId",
                table: "Embeddings",
                column: "SegmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Lookups_LookupTypeId",
                table: "Lookups",
                column: "LookupTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeGenerations");

            migrationBuilder.DropTable(
                name: "Embeddings");

            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "LookupTypes");
        }
    }
}

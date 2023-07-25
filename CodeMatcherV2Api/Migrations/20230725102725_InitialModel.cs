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
                name: "CodeGenerationRequests",
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
                    table.PrimaryKey("PK_CodeGenerationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGenerationRequests_Lookups_RunTypeId",
                        column: x => x.RunTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CodeGenerationRequests_Lookups_SegmentTypeId",
                        column: x => x.SegmentTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmbeddingRequests",
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
                    table.PrimaryKey("PK_EmbeddingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmbeddingRequests_Lookups_EmbeddingFrequencyId",
                        column: x => x.EmbeddingFrequencyId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmbeddingRequests_Lookups_RunTypeId",
                        column: x => x.RunTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmbeddingRequests_Lookups_SegmentTypeId",
                        column: x => x.SegmentTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CodeGenerationResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ResponseMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeGenerationResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeGenerationResponses_CodeGenerationRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "CodeGenerationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmbeddingsResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ResponseMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmbeddingsResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmbeddingsResponses_EmbeddingRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "EmbeddingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeGenerationRequests_RunTypeId",
                table: "CodeGenerationRequests",
                column: "RunTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGenerationRequests_SegmentTypeId",
                table: "CodeGenerationRequests",
                column: "SegmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeGenerationResponses_RequestId",
                table: "CodeGenerationResponses",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EmbeddingRequests_EmbeddingFrequencyId",
                table: "EmbeddingRequests",
                column: "EmbeddingFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmbeddingRequests_RunTypeId",
                table: "EmbeddingRequests",
                column: "RunTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmbeddingRequests_SegmentTypeId",
                table: "EmbeddingRequests",
                column: "SegmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmbeddingsResponses_RequestId",
                table: "EmbeddingsResponses",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Lookups_LookupTypeId",
                table: "Lookups",
                column: "LookupTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeGenerationResponses");

            migrationBuilder.DropTable(
                name: "EmbeddingsResponses");

            migrationBuilder.DropTable(
                name: "CodeGenerationRequests");

            migrationBuilder.DropTable(
                name: "EmbeddingRequests");

            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "LookupTypes");
        }
    }
}

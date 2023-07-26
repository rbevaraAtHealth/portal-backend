using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMappingEfCore.Migrations
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
                    LookupTypeKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LookupTypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "CodeMappingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunTypeId = table.Column<int>(type: "int", nullable: false),
                    SegmentTypeId = table.Column<int>(type: "int", nullable: false),
                    CodeMappingId = table.Column<int>(type: "int", nullable: false),
                    RunSchedule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Threshold = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatestLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeMappingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeMappingRequests_Lookups_CodeMappingId",
                        column: x => x.CodeMappingId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CodeMappingRequests_Lookups_RunTypeId",
                        column: x => x.RunTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CodeMappingRequests_Lookups_SegmentTypeId",
                        column: x => x.SegmentTypeId,
                        principalTable: "Lookups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CodeMappingResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ResponseMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeMappingResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeMappingResponses_CodeMappingRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "CodeMappingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodeMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Progress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeMappings_CodeMappingRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "CodeMappingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeMappingRequests_CodeMappingId",
                table: "CodeMappingRequests",
                column: "CodeMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeMappingRequests_RunTypeId",
                table: "CodeMappingRequests",
                column: "RunTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeMappingRequests_SegmentTypeId",
                table: "CodeMappingRequests",
                column: "SegmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeMappingResponses_RequestId",
                table: "CodeMappingResponses",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeMappings_RequestId",
                table: "CodeMappings",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Lookups_LookupTypeId",
                table: "Lookups",
                column: "LookupTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeMappingResponses");

            migrationBuilder.DropTable(
                name: "CodeMappings");

            migrationBuilder.DropTable(
                name: "CodeMappingRequests");

            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "LookupTypes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMatcher.EntityFrameworkCore.Migrations
{
    public partial class cgSummaryupdateforCsvUploadSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfNoiseRecords",
                table: "CodeGenerationSummary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UploadCsvOutputDirPath",
                table: "CodeGenerationSummary",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfNoiseRecords",
                table: "CodeGenerationSummary");

            migrationBuilder.DropColumn(
                name: "UploadCsvOutputDirPath",
                table: "CodeGenerationSummary");
        }
    }
}

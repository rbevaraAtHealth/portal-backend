using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMatcher.EntityFrameworkCore.Migrations
{
    public partial class Codemapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CsvOutputDirectoryPath",
                table: "CodeMappings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResponseMessage",
                table: "CodeMappingResponses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CsvOutputDirectoryPath",
                table: "CodeMappings");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseMessage",
                table: "CodeMappingResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

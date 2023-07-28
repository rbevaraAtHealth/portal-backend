using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMappingEfCore.Migrations
{
    public partial class RequestModelClientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "CodeMappingRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "CodeMappingRequests");
        }
    }
}

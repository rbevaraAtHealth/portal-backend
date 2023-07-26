using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeMappingEfCore.Migrations
{
    public partial class Script_BaseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFile = Path.Combine(Environment.CurrentDirectory, @"..\CodeMappingEfCore\Migrations\Scripts\");

            foreach (var file in Directory.GetFiles(sqlFile))
            {
                if (file.ToLower().Contains("sql"))
                {
                    migrationBuilder.Sql(File.ReadAllText(file));
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

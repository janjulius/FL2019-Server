using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Migrations
{
    public partial class _201964_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnedCharactersString",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnedCharactersString",
                table: "User");
        }
    }
}

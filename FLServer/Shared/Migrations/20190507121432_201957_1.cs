using Microsoft.EntityFrameworkCore.Migrations;

namespace FL_Login_Server.Migrations
{
    public partial class _201957_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueIdentifier",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueIdentifier",
                table: "User");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace FLServer.Migrations
{
    public partial class _2019_4_30_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Elo",
                table: "User",
                newName: "RankedElo");

            migrationBuilder.AddColumn<int>(
                name: "NormalElo",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalElo",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "RankedElo",
                table: "User",
                newName: "Elo");
        }
    }
}

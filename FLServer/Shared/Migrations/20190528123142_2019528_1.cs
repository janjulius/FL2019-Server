using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Migrations
{
    public partial class _2019528_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "Player",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_CharacterId",
                table: "Player",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Character_CharacterId",
                table: "Player",
                column: "CharacterId",
                principalTable: "Character",
                principalColumn: "CharacterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Character_CharacterId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_CharacterId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Player");
        }
    }
}

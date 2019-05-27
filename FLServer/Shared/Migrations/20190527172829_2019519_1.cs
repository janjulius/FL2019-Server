using Microsoft.EntityFrameworkCore.Migrations;

namespace FL_Login_Server.Migrations
{
    public partial class _2019519_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Player_PlayerFK",
                table: "Character");

            migrationBuilder.DropIndex(
                name: "IX_Passive_CharacterFK",
                table: "Passive");

            migrationBuilder.DropIndex(
                name: "IX_Character_PlayerFK",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "PlayerFK",
                table: "Character");

            migrationBuilder.CreateIndex(
                name: "IX_Passive_CharacterFK",
                table: "Passive",
                column: "CharacterFK",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Passive_CharacterFK",
                table: "Passive");

            migrationBuilder.AddColumn<int>(
                name: "PlayerFK",
                table: "Character",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Passive_CharacterFK",
                table: "Passive",
                column: "CharacterFK");

            migrationBuilder.CreateIndex(
                name: "IX_Character_PlayerFK",
                table: "Character",
                column: "PlayerFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Player_PlayerFK",
                table: "Character",
                column: "PlayerFK",
                principalTable: "Player",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

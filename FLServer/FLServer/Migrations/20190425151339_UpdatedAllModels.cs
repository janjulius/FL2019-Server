using Microsoft.EntityFrameworkCore.Migrations;

namespace FLServer.Migrations
{
    public partial class UpdatedAllModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchFK",
                table: "Team",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MapFK",
                table: "Map",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameModeFK",
                table: "Gamemode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerFK",
                table: "Character",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Team_MatchFK",
                table: "Team",
                column: "MatchFK");

            migrationBuilder.CreateIndex(
                name: "IX_Map_MapFK",
                table: "Map",
                column: "MapFK");

            migrationBuilder.CreateIndex(
                name: "IX_Gamemode_GameModeFK",
                table: "Gamemode",
                column: "GameModeFK");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Gamemode_Match_GameModeFK",
                table: "Gamemode",
                column: "GameModeFK",
                principalTable: "Match",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Map_Match_MapFK",
                table: "Map",
                column: "MapFK",
                principalTable: "Match",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Match_MatchFK",
                table: "Team",
                column: "MatchFK",
                principalTable: "Match",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Player_PlayerFK",
                table: "Character");

            migrationBuilder.DropForeignKey(
                name: "FK_Gamemode_Match_GameModeFK",
                table: "Gamemode");

            migrationBuilder.DropForeignKey(
                name: "FK_Map_Match_MapFK",
                table: "Map");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Match_MatchFK",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_MatchFK",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Map_MapFK",
                table: "Map");

            migrationBuilder.DropIndex(
                name: "IX_Gamemode_GameModeFK",
                table: "Gamemode");

            migrationBuilder.DropIndex(
                name: "IX_Character_PlayerFK",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "MatchFK",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "MapFK",
                table: "Map");

            migrationBuilder.DropColumn(
                name: "GameModeFK",
                table: "Gamemode");

            migrationBuilder.DropColumn(
                name: "PlayerFK",
                table: "Character");
        }
    }
}

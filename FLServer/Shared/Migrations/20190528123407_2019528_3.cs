using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Migrations
{
    public partial class _2019528_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gamemode_Match_GameModeFK",
                table: "Gamemode");

            migrationBuilder.DropForeignKey(
                name: "FK_Map_Match_MapFK",
                table: "Map");

            migrationBuilder.DropIndex(
                name: "IX_Map_MapFK",
                table: "Map");

            migrationBuilder.DropIndex(
                name: "IX_Gamemode_GameModeFK",
                table: "Gamemode");

            migrationBuilder.DropColumn(
                name: "GameMode",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "Map",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "MapFK",
                table: "Map");

            migrationBuilder.DropColumn(
                name: "GameModeFK",
                table: "Gamemode");

            migrationBuilder.AddColumn<int>(
                name: "GamemodeId",
                table: "Match",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapId",
                table: "Match",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Match_GamemodeId",
                table: "Match",
                column: "GamemodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_MapId",
                table: "Match",
                column: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Gamemode_GamemodeId",
                table: "Match",
                column: "GamemodeId",
                principalTable: "Gamemode",
                principalColumn: "GamemodeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Map_MapId",
                table: "Match",
                column: "MapId",
                principalTable: "Map",
                principalColumn: "MapId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Gamemode_GamemodeId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Map_MapId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_GamemodeId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_MapId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "GamemodeId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "Match");

            migrationBuilder.AddColumn<string>(
                name: "GameMode",
                table: "Match",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Map",
                table: "Match",
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

            migrationBuilder.CreateIndex(
                name: "IX_Map_MapFK",
                table: "Map",
                column: "MapFK");

            migrationBuilder.CreateIndex(
                name: "IX_Gamemode_GameModeFK",
                table: "Gamemode",
                column: "GameModeFK");

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
        }
    }
}

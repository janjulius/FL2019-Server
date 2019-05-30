using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Migrations
{
    public partial class _2019528_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Match_MatchFK",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_MatchFK",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "MatchFK",
                table: "Team");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Player",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_TeamId",
                table: "Player",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Team_TeamId",
                table: "Player",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Team_TeamId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_TeamId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "MatchFK",
                table: "Team",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Team_MatchFK",
                table: "Team",
                column: "MatchFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Match_MatchFK",
                table: "Team",
                column: "MatchFK",
                principalTable: "Match",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

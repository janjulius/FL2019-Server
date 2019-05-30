using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Migrations
{
    public partial class _2019528_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "Team",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_MatchId",
                table: "Team",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Match_MatchId",
                table: "Team",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Match_MatchId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_MatchId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Team");
        }
    }
}

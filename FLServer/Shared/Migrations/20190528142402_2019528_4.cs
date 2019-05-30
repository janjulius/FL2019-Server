using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Migrations
{
    public partial class _2019528_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatsId",
                table: "Player",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    StatsId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DamageDealt = table.Column<int>(nullable: false),
                    DamageTaken = table.Column<int>(nullable: false),
                    HighestPercentage = table.Column<int>(nullable: false),
                    Kills = table.Column<int>(nullable: false),
                    Deaths = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.StatsId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_StatsId",
                table: "Player",
                column: "StatsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Stats_StatsId",
                table: "Player",
                column: "StatsId",
                principalTable: "Stats",
                principalColumn: "StatsId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Stats_StatsId",
                table: "Player");

            migrationBuilder.DropTable(
                name: "Stats");

            migrationBuilder.DropIndex(
                name: "IX_Player_StatsId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "StatsId",
                table: "Player");
        }
    }
}

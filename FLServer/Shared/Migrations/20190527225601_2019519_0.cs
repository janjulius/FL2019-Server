using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Migrations
{
    public partial class _2019519_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterAbility");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId1",
                table: "Ability",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ability_CharacterId1",
                table: "Ability",
                column: "CharacterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ability_Character_CharacterId1",
                table: "Ability",
                column: "CharacterId1",
                principalTable: "Character",
                principalColumn: "CharacterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ability_Character_CharacterId1",
                table: "Ability");

            migrationBuilder.DropIndex(
                name: "IX_Ability_CharacterId1",
                table: "Ability");

            migrationBuilder.DropColumn(
                name: "CharacterId1",
                table: "Ability");

            migrationBuilder.CreateTable(
                name: "CharacterAbility",
                columns: table => new
                {
                    CharacterAbilityId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AbilityId = table.Column<int>(nullable: false),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAbility", x => x.CharacterAbilityId);
                    table.ForeignKey(
                        name: "FK_CharacterAbility_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAbility_CharacterId",
                table: "CharacterAbility",
                column: "CharacterId");
        }
    }
}

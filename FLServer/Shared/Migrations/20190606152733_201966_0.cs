using Microsoft.EntityFrameworkCore.Migrations;

namespace Shared.Migrations
{
    public partial class _201966_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_User1UserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_User2UserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_User1UserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_User2UserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "User1UserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "User2UserId",
                table: "Message");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "FriendRequest",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ReceiverId",
                table: "Message",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_ReceiverId",
                table: "Message",
                column: "ReceiverId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_ReceiverId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_SenderId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ReceiverId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_SenderId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "FriendRequest");

            migrationBuilder.AddColumn<int>(
                name: "User1UserId",
                table: "Message",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "User2UserId",
                table: "Message",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_User1UserId",
                table: "Message",
                column: "User1UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_User2UserId",
                table: "Message",
                column: "User2UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_User1UserId",
                table: "Message",
                column: "User1UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_User2UserId",
                table: "Message",
                column: "User2UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

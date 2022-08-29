using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oversee.Migrations
{
    public partial class UserConnectionRequestUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "UserConnectionRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReceivingUserId",
                table: "UserConnectionRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SendingUserId",
                table: "UserConnectionRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnectionRequests_ReceivingUserId",
                table: "UserConnectionRequests",
                column: "ReceivingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnectionRequests_SendingUserId",
                table: "UserConnectionRequests",
                column: "SendingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnectionRequests_AspNetUsers_ReceivingUserId",
                table: "UserConnectionRequests",
                column: "ReceivingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnectionRequests_AspNetUsers_SendingUserId",
                table: "UserConnectionRequests",
                column: "SendingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnectionRequests_AspNetUsers_ReceivingUserId",
                table: "UserConnectionRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConnectionRequests_AspNetUsers_SendingUserId",
                table: "UserConnectionRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserConnectionRequests_ReceivingUserId",
                table: "UserConnectionRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserConnectionRequests_SendingUserId",
                table: "UserConnectionRequests");

            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "UserConnectionRequests");

            migrationBuilder.DropColumn(
                name: "ReceivingUserId",
                table: "UserConnectionRequests");

            migrationBuilder.DropColumn(
                name: "SendingUserId",
                table: "UserConnectionRequests");
        }
    }
}

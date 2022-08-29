using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oversee.Migrations
{
    public partial class UserConnectionRequestUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "UserConnectionRequests");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "UserConnectionRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "UserConnectionRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "UserConnectionRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}

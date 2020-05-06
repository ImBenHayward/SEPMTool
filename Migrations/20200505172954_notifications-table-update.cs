using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SEPMTool.Migrations
{
    public partial class notificationstableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "NotificationUser",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "NotificationUser");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                nullable: false,
                defaultValue: false);
        }
    }
}

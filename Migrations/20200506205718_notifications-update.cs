using Microsoft.EntityFrameworkCore.Migrations;

namespace SEPMTool.Migrations
{
    public partial class notificationsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectLink",
                table: "Notifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserLink",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectLink",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UserLink",
                table: "Notifications");
        }
    }
}

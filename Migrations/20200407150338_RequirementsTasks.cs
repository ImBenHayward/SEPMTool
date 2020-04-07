using Microsoft.EntityFrameworkCore.Migrations;

namespace SEPMTool.Migrations
{
    public partial class RequirementsTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Tasks",
                newName: "ProjectRequirementId");

            migrationBuilder.RenameColumn(
                name: "ProjectTaskId",
                table: "Tasks",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectRequirementId");

            migrationBuilder.RenameColumn(
                name: "ProjectRequirementId",
                table: "Requirements",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Requirements_ProjectRequirementId",
                table: "Tasks",
                column: "ProjectRequirementId",
                principalTable: "Requirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Requirements_ProjectRequirementId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "ProjectRequirementId",
                table: "Tasks",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tasks",
                newName: "ProjectTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ProjectRequirementId",
                table: "Tasks",
                newName: "IX_Tasks_ProjectId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Requirements",
                newName: "ProjectRequirementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

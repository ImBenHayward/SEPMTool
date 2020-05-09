using Microsoft.EntityFrameworkCore.Migrations;

namespace SEPMTool.Migrations
{
    public partial class updatefluentcommentstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectRequirementId",
                table: "CommentLikes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_ProjectRequirementId",
                table: "CommentLikes",
                column: "ProjectRequirementId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Requirements_ProjectRequirementId",
                table: "CommentLikes",
                column: "ProjectRequirementId",
                principalTable: "Requirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Requirements_ProjectRequirementId",
                table: "CommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_CommentLikes_ProjectRequirementId",
                table: "CommentLikes");

            migrationBuilder.DropColumn(
                name: "ProjectRequirementId",
                table: "CommentLikes");
        }
    }
}

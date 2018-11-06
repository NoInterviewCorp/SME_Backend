using Microsoft.EntityFrameworkCore.Migrations;

namespace Module_SME.Migrations
{
    public partial class QuestionLinkToTechnology : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TechnologyId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TechnologyId",
                table: "Questions",
                column: "TechnologyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Technologies_TechnologyId",
                table: "Questions",
                column: "TechnologyId",
                principalTable: "Technologies",
                principalColumn: "TechnologyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Technologies_TechnologyId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TechnologyId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TechnologyId",
                table: "Questions");
        }
    }
}

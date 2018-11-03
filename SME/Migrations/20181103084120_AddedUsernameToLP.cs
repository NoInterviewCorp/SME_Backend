using Microsoft.EntityFrameworkCore.Migrations;

namespace Module_SME.Migrations
{
    public partial class AddedUsernameToLP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "LearningPlan",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "LearningPlan");
        }
    }
}

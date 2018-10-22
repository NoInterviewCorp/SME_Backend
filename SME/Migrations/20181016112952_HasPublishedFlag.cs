using Microsoft.EntityFrameworkCore.Migrations;

namespace Module_SME.Migrations
{
    public partial class HasPublishedFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPublished",
                table: "Questions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPublished",
                table: "Questions");
        }
    }
}

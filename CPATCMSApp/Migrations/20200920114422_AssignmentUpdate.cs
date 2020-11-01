using Microsoft.EntityFrameworkCore.Migrations;

namespace CPATCMSApp.Migrations
{
    public partial class AssignmentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfAssignmentItems",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfAssignmentItems",
                table: "Assignments");
        }
    }
}

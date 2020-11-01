using Microsoft.EntityFrameworkCore.Migrations;

namespace CPATCMSApp.Migrations
{
    public partial class TruckEntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TruckNumer",
                table: "TruckEntities",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdCardNumberOfAssistant",
                table: "TruckEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdCardNumberOfDriver",
                table: "TruckEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCardNumberOfAssistant",
                table: "TruckEntities");

            migrationBuilder.DropColumn(
                name: "IdCardNumberOfDriver",
                table: "TruckEntities");

            migrationBuilder.AlterColumn<string>(
                name: "TruckNumer",
                table: "TruckEntities",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicUtilities.Data.Migrations
{
    public partial class AddNewFieldsForApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScaleOfWork",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeOfWork",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScaleOfWork",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "TypeOfWork",
                table: "Applications");
        }
    }
}

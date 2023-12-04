using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicUtilities.Data.Migrations
{
    public partial class InserDefaultWorkers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "Workers",
            columns: new[] { "FirstName", "LastName", },
            values: new object[] { "Анатолий", "Казаков", });

            migrationBuilder.InsertData(
            table: "Workers",
            columns: new[] { "FirstName", "LastName", },
            values: new object[] { "Иван", "Иванов", });

            migrationBuilder.InsertData(
            table: "Workers",
            columns: new[] { "FirstName", "LastName", },
            values: new object[] { "Пётр", "Петров", });

            migrationBuilder.InsertData(
            table: "Workers",
            columns: new[] { "FirstName", "LastName", },
            values: new object[] { "Денис", "Денисов", });

            migrationBuilder.InsertData(
            table: "Workers",
            columns: new[] { "FirstName", "LastName", },
            values: new object[] { "Максим", "Максимов", });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

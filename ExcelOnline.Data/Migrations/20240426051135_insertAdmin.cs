using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelOnline.Data.Migrations
{
    public partial class insertAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Name", "PhoneNumber", "Role" },
                values: new object[] { 1, "管理员", "1234567", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

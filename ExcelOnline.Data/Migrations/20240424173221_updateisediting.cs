using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelOnline.Data.Migrations
{
    public partial class updateisediting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEditing",
                table: "SaleStatus",
                type: "tinyint",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditing",
                table: "SaleStatus");
        }
    }
}

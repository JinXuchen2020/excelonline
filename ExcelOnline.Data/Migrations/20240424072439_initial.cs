using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelOnline.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaleStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(1024)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    ShopName = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    ContactJob = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    SalerName = table.Column<string>(type: "nvarchar(1024)", nullable: true),
                    SuccessfulRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LinkUpDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    BidDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    VisitDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    BidConfirmDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ContractDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RemarkDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleStatus");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Crypto.Infrastructure.Persistence.Migrations
{
    public partial class DataChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountOwnerName",
                table: "FinancialInformation");

            migrationBuilder.DropColumn(
                name: "BankCardImage",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "BankCardImageStatus",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "BankCardImage",
                table: "FinancialInformation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankCardImage",
                table: "FinancialInformation");

            migrationBuilder.AddColumn<string>(
                name: "AccountOwnerName",
                table: "FinancialInformation",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankCardImage",
                table: "Documents",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankCardImageStatus",
                table: "Documents",
                type: "int",
                nullable: true);
        }
    }
}

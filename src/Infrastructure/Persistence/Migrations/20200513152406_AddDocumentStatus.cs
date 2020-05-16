using Microsoft.EntityFrameworkCore.Migrations;

namespace Crypto.Infrastructure.Persistence.Migrations
{
    public partial class AddDocumentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicantImageStatus",
                table: "Documents",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BankCardImageStatus",
                table: "Documents",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NationalCardImageStatus",
                table: "Documents",
                nullable: true,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantImageStatus",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "BankCardImageStatus",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "NationalCardImageStatus",
                table: "Documents");
        }
    }
}

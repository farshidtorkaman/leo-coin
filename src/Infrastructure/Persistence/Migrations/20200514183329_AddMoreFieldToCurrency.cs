using Microsoft.EntityFrameworkCore.Migrations;

namespace Crypto.Infrastructure.Persistence.Migrations
{
    public partial class AddMoreFieldToCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanBePurchase",
                table: "Currencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanBeSell",
                table: "Currencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WalletId",
                table: "Currencies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanBePurchase",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "CanBeSell",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Currencies");
        }
    }
}

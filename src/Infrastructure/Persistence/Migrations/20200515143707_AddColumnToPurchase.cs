using Microsoft.EntityFrameworkCore.Migrations;

namespace Crypto.Infrastructure.Persistence.Migrations
{
    public partial class AddColumnToPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Purchases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Purchases");
        }
    }
}

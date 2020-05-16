using Microsoft.EntityFrameworkCore.Migrations;

namespace Crypto.Infrastructure.Persistence.Migrations
{
    public partial class AddDisplayUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayUrl",
                table: "Currencies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayUrl",
                table: "Currencies");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Crypto.Infrastructure.Persistence.Migrations
{
    public partial class AddUserIdToDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Documents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Documents");
        }
    }
}

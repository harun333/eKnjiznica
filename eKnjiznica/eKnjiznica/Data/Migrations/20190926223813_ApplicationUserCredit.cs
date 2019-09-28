using Microsoft.EntityFrameworkCore.Migrations;

namespace eKnjiznica.Data.Migrations
{
    public partial class ApplicationUserCredit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Credit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Credit",
                table: "AspNetUsers");
        }
    }
}

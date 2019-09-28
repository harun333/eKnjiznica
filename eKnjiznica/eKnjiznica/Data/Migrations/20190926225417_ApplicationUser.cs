using Microsoft.EntityFrameworkCore.Migrations;

namespace eKnjiznica.Data.Migrations
{
    public partial class ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Clients",
                newName: "applicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "applicationUserId",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_applicationUserId",
                table: "Clients",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AspNetUsers_applicationUserId",
                table: "Clients",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AspNetUsers_applicationUserId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_applicationUserId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "applicationUserId",
                table: "Clients",
                newName: "userId");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

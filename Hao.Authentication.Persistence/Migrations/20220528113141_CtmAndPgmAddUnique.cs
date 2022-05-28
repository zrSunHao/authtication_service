using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hao.Authentication.Persistence.Migrations
{
    public partial class CtmAndPgmAddUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sys_Code",
                table: "Sys",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Name",
                table: "Sys",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Program_Code",
                table: "Program",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Program_Name",
                table: "Program",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Name",
                table: "Customer",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sys_Code",
                table: "Sys");

            migrationBuilder.DropIndex(
                name: "IX_Sys_Name",
                table: "Sys");

            migrationBuilder.DropIndex(
                name: "IX_Program_Code",
                table: "Program");

            migrationBuilder.DropIndex(
                name: "IX_Program_Name",
                table: "Program");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Name",
                table: "Customer");
        }
    }
}

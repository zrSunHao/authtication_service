using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hao.Authentication.Persistence.Migrations
{
    public partial class PgmAddIntro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Intro",
                table: "Program",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Intro",
                table: "Program");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hao.Authentication.Persistence.Migrations
{
    public partial class AddLoginRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedById",
                table: "Constraint",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserLastLoginRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SysId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLastLoginRecord", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLastLoginRecord_CustomerId",
                table: "UserLastLoginRecord",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLastLoginRecord_RoleId",
                table: "UserLastLoginRecord",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLastLoginRecord_SysId",
                table: "UserLastLoginRecord",
                column: "SysId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLastLoginRecord");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedById",
                table: "Constraint",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32,
                oldNullable: true);
        }
    }
}

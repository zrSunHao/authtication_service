using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hao.Authentication.Persistence.Migrations
{
    public partial class AddEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Constraint",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TargetId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cancelled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constraint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInformation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Education = table.Column<int>(type: "int", nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Intro = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Operate = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SystemId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ProgramId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RemoteAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRoleRelation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRoleRelation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileResource",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OwnId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Suffix = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileResource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Program",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramFunction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SectionId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ProgramId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramFunction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramSection",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ProgramId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramSection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Intro = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysProgramRelation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SysId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ProgramId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysProgramRelation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SysId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Intro = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysRoleFuncRelation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ProgramId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TargetId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsFunction = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRoleFuncRelation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Constraint_TargetId",
                table: "Constraint",
                column: "TargetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInformation_CustomerId",
                table: "CustomerInformation",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLog_CustomerId",
                table: "CustomerLog",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoleRelation_CustomerId",
                table: "CustomerRoleRelation",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoleRelation_RoleId",
                table: "CustomerRoleRelation",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FileResource_Code",
                table: "FileResource",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileResource_OwnId",
                table: "FileResource",
                column: "OwnId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramFunction_SectionId",
                table: "ProgramFunction",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramSection_ProgramId",
                table: "ProgramSection",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_SysProgramRelation_ProgramId",
                table: "SysProgramRelation",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_SysProgramRelation_SysId",
                table: "SysProgramRelation",
                column: "SysId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRole_SysId",
                table: "SysRole",
                column: "SysId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRoleFuncRelation_RoleId",
                table: "SysRoleFuncRelation",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRoleFuncRelation_TargetId",
                table: "SysRoleFuncRelation",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Constraint");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerInformation");

            migrationBuilder.DropTable(
                name: "CustomerLog");

            migrationBuilder.DropTable(
                name: "CustomerRoleRelation");

            migrationBuilder.DropTable(
                name: "FileResource");

            migrationBuilder.DropTable(
                name: "Program");

            migrationBuilder.DropTable(
                name: "ProgramFunction");

            migrationBuilder.DropTable(
                name: "ProgramSection");

            migrationBuilder.DropTable(
                name: "Sys");

            migrationBuilder.DropTable(
                name: "SysProgramRelation");

            migrationBuilder.DropTable(
                name: "SysRole");

            migrationBuilder.DropTable(
                name: "SysRoleFuncRelation");
        }
    }
}

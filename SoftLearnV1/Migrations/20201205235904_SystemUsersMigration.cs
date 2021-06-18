using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class SystemUsersMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponCodes_SuperAdministrator_CreatedById",
                table: "CouponCodes");

            migrationBuilder.DropTable(
                name: "SuperAdministrator");

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemUserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemUserRoles_SystemRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemUserRoles_SystemUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SystemUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SystemRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[] { 1L, "Super Administrator" });

            migrationBuilder.InsertData(
                table: "SystemRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[] { 2L, "Administrator" });

            migrationBuilder.InsertData(
                table: "SystemRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[] { 3L, "Content Creator" });

            migrationBuilder.CreateIndex(
                name: "IX_SystemUserRoles_RoleId",
                table: "SystemUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUserRoles_UserId",
                table: "SystemUserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponCodes_SystemUsers_CreatedById",
                table: "CouponCodes",
                column: "CreatedById",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponCodes_SystemUsers_CreatedById",
                table: "CouponCodes");

            migrationBuilder.DropTable(
                name: "SystemUserRoles");

            migrationBuilder.DropTable(
                name: "SystemRoles");

            migrationBuilder.DropTable(
                name: "SystemUsers");

            migrationBuilder.CreateTable(
                name: "SuperAdministrator",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdministrator", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CouponCodes_SuperAdministrator_CreatedById",
                table: "CouponCodes",
                column: "CreatedById",
                principalTable: "SuperAdministrator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

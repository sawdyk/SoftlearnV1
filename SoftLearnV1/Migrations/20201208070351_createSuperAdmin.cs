using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class createSuperAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "IsActive", "LastLoginDate", "LastName", "LastPasswordChangedDate", "LastUpdatedDate", "PasswordHash", "Salt", "UserName" },
                values: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new DateTime(2020, 12, 8, 8, 3, 50, 91, DateTimeKind.Local).AddTicks(4510), "superadmin@superadmin.com", "Super", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "a6737847d4255354d8e3688e02e06181f84b167bd09e83e0b5f5ed6e19f88544", null, "superadmin@superadmin.com" });

            migrationBuilder.InsertData(
                table: "SystemUserRoles",
                columns: new[] { "Id", "DateCreated", "RoleId", "UserId" },
                values: new object[] { 1L, new DateTime(2020, 12, 8, 8, 3, 50, 102, DateTimeKind.Local).AddTicks(792), 1L, new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"));
        }
    }
}

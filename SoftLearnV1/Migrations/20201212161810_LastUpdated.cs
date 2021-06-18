using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class LastUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "SchoolTemporaryPayments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "SchoolFeesPayments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "FeeTemplateList",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "FeeSubCategory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "FeeCategory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "SchoolTemporaryPayments");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "SchoolFeesPayments");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "FeeTemplateList");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "FeeSubCategory");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "FeeCategory");

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "IsActive", "LastLoginDate", "LastName", "LastPasswordChangedDate", "LastUpdatedDate", "PasswordHash", "Salt", "UserName" },
                values: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new DateTime(2020, 12, 11, 9, 17, 0, 193, DateTimeKind.Local).AddTicks(3708), "superadmin@superadmin.com", "Super", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 12, 11, 9, 17, 0, 200, DateTimeKind.Local).AddTicks(9553), "2ff36c8a963745601f1c5547a8ae367177435806b7b03bec128efae0c2413e7c", "658d14ca825a34559a6b485a10b5b3b3", "superadmin@superadmin.com" });

            migrationBuilder.InsertData(
                table: "SystemUserRoles",
                columns: new[] { "Id", "DateCreated", "RoleId", "UserId" },
                values: new object[] { 1L, new DateTime(2020, 12, 11, 9, 17, 0, 201, DateTimeKind.Local).AddTicks(1490), 1L, new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575") });
        }
    }
}

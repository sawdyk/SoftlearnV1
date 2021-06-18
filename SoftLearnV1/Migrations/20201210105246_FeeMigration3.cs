using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class FeeMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 10, 11, 52, 45, 123, DateTimeKind.Local).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 10, 11, 52, 45, 111, DateTimeKind.Local).AddTicks(9000), new DateTime(2020, 12, 10, 11, 52, 45, 123, DateTimeKind.Local).AddTicks(1327), "770613f93df1f832d97e680dd8f1b264901884f08da7547107b691c5cc0d4971", "8e32a75870d3e1b6d314759128263ad0" });

            migrationBuilder.CreateIndex(
                name: "IX_Facilitators_FacilitatorTypeId",
                table: "Facilitators",
                column: "FacilitatorTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilitators_FacilitatorType_FacilitatorTypeId",
                table: "Facilitators",
                column: "FacilitatorTypeId",
                principalTable: "FacilitatorType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facilitators_FacilitatorType_FacilitatorTypeId",
                table: "Facilitators");

            migrationBuilder.DropIndex(
                name: "IX_Facilitators_FacilitatorTypeId",
                table: "Facilitators");

            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 10, 10, 51, 26, 705, DateTimeKind.Local).AddTicks(2166));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 10, 10, 51, 26, 699, DateTimeKind.Local).AddTicks(7058), new DateTime(2020, 12, 10, 10, 51, 26, 705, DateTimeKind.Local).AddTicks(1610), "5d7e4c3d03e5c4cfec4935d08f0e8489a3d8dc491fc2cdf3c4467eab9458e4c6", "6820f737d4d59e81c5f4a4ee9f33826b" });
        }
    }
}

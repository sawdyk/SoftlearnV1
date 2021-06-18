using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class FeeMigration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FacilitatorTypeId",
                table: "Facilitators",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 10, 12, 19, 27, 655, DateTimeKind.Local).AddTicks(4446));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 10, 12, 19, 27, 650, DateTimeKind.Local).AddTicks(7759), new DateTime(2020, 12, 10, 12, 19, 27, 655, DateTimeKind.Local).AddTicks(3973), "ea094a96c7f3891d91b83e88d2a5cf9c55379de778f43c4db3040baf1920cb1e", "2e60d65f220079e54bf7e9ec0c2fb028" });

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

            migrationBuilder.DropColumn(
                name: "FacilitatorTypeId",
                table: "Facilitators");

            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 10, 12, 15, 12, 121, DateTimeKind.Local).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 10, 12, 15, 12, 111, DateTimeKind.Local).AddTicks(7554), new DateTime(2020, 12, 10, 12, 15, 12, 121, DateTimeKind.Local).AddTicks(8745), "a4bdfda5b977fdb92627fe4ad9e47cd1ab4b9c12606d55583cb3592a9a96f2ba", "503f1943d655d2cbcb1552fe524b0127" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class FeeMigration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                value: new DateTime(2020, 12, 10, 10, 1, 32, 654, DateTimeKind.Local).AddTicks(6595));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 10, 10, 1, 32, 648, DateTimeKind.Local).AddTicks(9062), new DateTime(2020, 12, 10, 10, 1, 32, 654, DateTimeKind.Local).AddTicks(6079), "220f719fd3f8f35cf4932dafaade30488e7f203cf60c53fd6e972cc9545b0310", "ece71ce0e11fbef7540e7304248c67e5" });

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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class SystemUsersMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponCodes_SystemUsers_CreatedById",
                table: "CouponCodes");

            migrationBuilder.DropColumn(
                name: "SuperAdminId",
                table: "CouponCodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "CouponCodes",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CouponCodes_SystemUsers_CreatedById",
                table: "CouponCodes",
                column: "CreatedById",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponCodes_SystemUsers_CreatedById",
                table: "CouponCodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "CouponCodes",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "SuperAdminId",
                table: "CouponCodes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_CouponCodes_SystemUsers_CreatedById",
                table: "CouponCodes",
                column: "CreatedById",
                principalTable: "SystemUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

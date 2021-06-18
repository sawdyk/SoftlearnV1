using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                table: "SchoolTemporaryPayments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "SchoolTemporaryPayments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "VerifiedBy",
                table: "SchoolTemporaryPayments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "SchoolTemporaryPayments");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "SchoolTemporaryPayments");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "SchoolTemporaryPayments");
        }
    }
}

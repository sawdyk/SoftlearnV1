using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class studentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Students",
                newName: "YearOfAdmission");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Students",
                newName: "StudentStatus");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Gender",
                table: "Students",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalGovt",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StaffStatus",
                table: "Students",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateOfOrigin",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Parents",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Gender",
                table: "Parents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "HomeAddress",
                table: "Parents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalGovt",
                table: "Parents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Parents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Parents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Religion",
                table: "Parents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Parents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateOfOrigin",
                table: "Parents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "HomeAddress",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LocalGovt",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StaffStatus",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StateOfOrigin",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "HomeAddress",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "LocalGovt",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Religion",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "StateOfOrigin",
                table: "Parents");

            migrationBuilder.RenameColumn(
                name: "YearOfAdmission",
                table: "Students",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "StudentStatus",
                table: "Students",
                newName: "Email");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Students",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Students",
                nullable: false,
                defaultValue: false);
        }
    }
}

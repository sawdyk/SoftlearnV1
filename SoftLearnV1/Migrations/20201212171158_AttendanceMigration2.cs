using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class AttendanceMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeTeachers_Sessions_SessionId",
                table: "GradeTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeTeachers_Terms_TermId",
                table: "GradeTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeachers_Sessions_SessionId",
                table: "SubjectTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeachers_Terms_TermId",
                table: "SubjectTeachers");

            migrationBuilder.AlterColumn<long>(
                name: "TermId",
                table: "SubjectTeachers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SessionId",
                table: "SubjectTeachers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TermId",
                table: "GradeTeachers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SessionId",
                table: "GradeTeachers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 12, 18, 11, 55, 548, DateTimeKind.Local).AddTicks(4158));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 12, 18, 11, 55, 218, DateTimeKind.Local).AddTicks(2443), new DateTime(2020, 12, 12, 18, 11, 55, 548, DateTimeKind.Local).AddTicks(2991), "dfa06c1272d3c04b7d3800d7cc42a4ce1a8df5d7c31550328e1577333a529cda", "34d6599cf24fa0a6e2f3ee71d6b79ce9" });

            migrationBuilder.AddForeignKey(
                name: "FK_GradeTeachers_Sessions_SessionId",
                table: "GradeTeachers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeTeachers_Terms_TermId",
                table: "GradeTeachers",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeachers_Sessions_SessionId",
                table: "SubjectTeachers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeachers_Terms_TermId",
                table: "SubjectTeachers",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeTeachers_Sessions_SessionId",
                table: "GradeTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_GradeTeachers_Terms_TermId",
                table: "GradeTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeachers_Sessions_SessionId",
                table: "SubjectTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeachers_Terms_TermId",
                table: "SubjectTeachers");

            migrationBuilder.AlterColumn<long>(
                name: "TermId",
                table: "SubjectTeachers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "SessionId",
                table: "SubjectTeachers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "TermId",
                table: "GradeTeachers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "SessionId",
                table: "GradeTeachers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 12, 17, 37, 57, 425, DateTimeKind.Local).AddTicks(341));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 12, 17, 37, 57, 412, DateTimeKind.Local).AddTicks(3559), new DateTime(2020, 12, 12, 17, 37, 57, 424, DateTimeKind.Local).AddTicks(9304), "cf3514d0e2d107e4637766cce1864012a528dfdbf9ea11b1af3b16fac790b42f", "2dac492f8c218139071fd2acdb77ea94" });

            migrationBuilder.AddForeignKey(
                name: "FK_GradeTeachers_Sessions_SessionId",
                table: "GradeTeachers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GradeTeachers_Terms_TermId",
                table: "GradeTeachers",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeachers_Sessions_SessionId",
                table: "SubjectTeachers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeachers_Terms_TermId",
                table: "SubjectTeachers",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

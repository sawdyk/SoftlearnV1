using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class LessonNoteMigration : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_SubjectTeachers_SessionId",
                table: "SubjectTeachers");

            migrationBuilder.DropIndex(
                name: "IX_SubjectTeachers_TermId",
                table: "SubjectTeachers");

            migrationBuilder.DropIndex(
                name: "IX_GradeTeachers_SessionId",
                table: "GradeTeachers");

            migrationBuilder.DropIndex(
                name: "IX_GradeTeachers_TermId",
                table: "GradeTeachers");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "SubjectTeachers");

            migrationBuilder.DropColumn(
                name: "TermId",
                table: "SubjectTeachers");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "GradeTeachers");

            migrationBuilder.DropColumn(
                name: "TermId",
                table: "GradeTeachers");

            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 14, 14, 11, 42, 453, DateTimeKind.Local).AddTicks(8212));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 14, 14, 11, 42, 413, DateTimeKind.Local).AddTicks(6721), new DateTime(2020, 12, 14, 14, 11, 42, 453, DateTimeKind.Local).AddTicks(7720), "b3b9fecc93e42c8ae020a2c5e4bbb8d69224c0ed255e32b84cbc71a8203ea3e0", "40cd05d1bdb7230c761f97a5dea037eb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SessionId",
                table: "SubjectTeachers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TermId",
                table: "SubjectTeachers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SessionId",
                table: "GradeTeachers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TermId",
                table: "GradeTeachers",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_SessionId",
                table: "SubjectTeachers",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_TermId",
                table: "SubjectTeachers",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_SessionId",
                table: "GradeTeachers",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_TermId",
                table: "GradeTeachers",
                column: "TermId");

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
    }
}

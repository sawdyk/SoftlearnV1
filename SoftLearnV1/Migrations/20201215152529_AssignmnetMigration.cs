using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class AssignmnetMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "LessonNotes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AssignmentsSubmitted");

            migrationBuilder.RenameColumn(
                name: "MarkStatus",
                table: "AssignmentsSubmitted",
                newName: "ScoreStatusId");

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "LessonNotes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<decimal>(
                name: "ScoreObtained",
                table: "AssignmentsSubmitted",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "ObtainableScore",
                table: "AssignmentsSubmitted",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSubmitted",
                table: "AssignmentsSubmitted",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateGraded",
                table: "AssignmentsSubmitted",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "AssignmentsSubmitted",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ScoreStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ScoreStatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ScoreStatus",
                columns: new[] { "Id", "ScoreStatusName" },
                values: new object[,]
                {
                    { 1L, "Passed" },
                    { 2L, "Failed" },
                    { 3L, "Pending" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    { 1L, "Approved" },
                    { 2L, "Pending" },
                    { 3L, "Declined" }
                });

            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 15, 16, 25, 28, 48, DateTimeKind.Local).AddTicks(9229));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 15, 16, 25, 27, 538, DateTimeKind.Local).AddTicks(4405), new DateTime(2020, 12, 15, 16, 25, 28, 48, DateTimeKind.Local).AddTicks(8719), "e54b729420b96a6362b57ee8cc9b7ab17d9df3a7b6c56fefd9231480d1e9e565", "e40ee543b2cc23e4be6b083fd55fb837" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_StatusId",
                table: "LessonNotes",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_ScoreStatusId",
                table: "AssignmentsSubmitted",
                column: "ScoreStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_StudentId",
                table: "AssignmentsSubmitted",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentsSubmitted_ScoreStatus_ScoreStatusId",
                table: "AssignmentsSubmitted",
                column: "ScoreStatusId",
                principalTable: "ScoreStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentsSubmitted_Students_StudentId",
                table: "AssignmentsSubmitted",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonNotes_Status_StatusId",
                table: "LessonNotes",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentsSubmitted_ScoreStatus_ScoreStatusId",
                table: "AssignmentsSubmitted");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentsSubmitted_Students_StudentId",
                table: "AssignmentsSubmitted");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonNotes_Status_StatusId",
                table: "LessonNotes");

            migrationBuilder.DropTable(
                name: "ScoreStatus");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_LessonNotes_StatusId",
                table: "LessonNotes");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentsSubmitted_ScoreStatusId",
                table: "AssignmentsSubmitted");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentsSubmitted_StudentId",
                table: "AssignmentsSubmitted");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "LessonNotes");

            migrationBuilder.DropColumn(
                name: "DateGraded",
                table: "AssignmentsSubmitted");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AssignmentsSubmitted");

            migrationBuilder.RenameColumn(
                name: "ScoreStatusId",
                table: "AssignmentsSubmitted",
                newName: "MarkStatus");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "LessonNotes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "ScoreObtained",
                table: "AssignmentsSubmitted",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ObtainableScore",
                table: "AssignmentsSubmitted",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSubmitted",
                table: "AssignmentsSubmitted",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AssignmentsSubmitted",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "DateCreated",
                value: new DateTime(2020, 12, 14, 14, 37, 21, 215, DateTimeKind.Local).AddTicks(7706));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 14, 14, 37, 21, 210, DateTimeKind.Local).AddTicks(6428), new DateTime(2020, 12, 14, 14, 37, 21, 215, DateTimeKind.Local).AddTicks(7197), "f622e4b957840db64e0c2462945a28d73ab859fd7de9c0d42b3221495307fc03", "657a6fe779879a24049146594c55adb1" });
        }
    }
}

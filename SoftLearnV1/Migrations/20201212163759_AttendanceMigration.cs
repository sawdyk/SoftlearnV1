using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class AttendanceMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SessionId",
                table: "SubjectTeachers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TermId",
                table: "SubjectTeachers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SessionId",
                table: "GradeTeachers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TermId",
                table: "GradeTeachers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttendancePeriod",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttendancePeriodName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancePeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentAttendance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    StudentId = table.Column<Guid>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    PresentAbsent = table.Column<long>(nullable: false),
                    AttendancePeriodId = table.Column<long>(nullable: false),
                    AttendancePeriodIdMorning = table.Column<long>(nullable: false),
                    AttendancePeriodIdAfternoon = table.Column<long>(nullable: false),
                    AttendanceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_AttendancePeriod_AttendancePeriodId",
                        column: x => x.AttendancePeriodId,
                        principalTable: "AttendancePeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AttendancePeriod",
                columns: new[] { "Id", "AttendancePeriodName" },
                values: new object[,]
                {
                    { 1L, "Morning" },
                    { 2L, "Afternoon" },
                    { 3L, "Both" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_AttendancePeriodId",
                table: "StudentAttendance",
                column: "AttendancePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_CampusId",
                table: "StudentAttendance",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_ClassGradeId",
                table: "StudentAttendance",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_ClassId",
                table: "StudentAttendance",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_SchoolId",
                table: "StudentAttendance",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_SessionId",
                table: "StudentAttendance",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_StudentId",
                table: "StudentAttendance",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_TermId",
                table: "StudentAttendance",
                column: "TermId");

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

            migrationBuilder.DropTable(
                name: "StudentAttendance");

            migrationBuilder.DropTable(
                name: "AttendancePeriod");

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
                value: new DateTime(2020, 12, 10, 12, 19, 27, 655, DateTimeKind.Local).AddTicks(4446));

            migrationBuilder.UpdateData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"),
                columns: new[] { "DateCreated", "LastUpdatedDate", "PasswordHash", "Salt" },
                values: new object[] { new DateTime(2020, 12, 10, 12, 19, 27, 650, DateTimeKind.Local).AddTicks(7759), new DateTime(2020, 12, 10, 12, 19, 27, 655, DateTimeKind.Local).AddTicks(3973), "ea094a96c7f3891d91b83e88d2a5cf9c55379de778f43c4db3040baf1920cb1e", "2e60d65f220079e54bf7e9ec0c2fb028" });
        }
    }
}

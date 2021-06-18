using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class LessonNoteMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    ObtainableScore = table.Column<long>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    SubjectId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_SchoolUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonNotes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    SubjectId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonNotes_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_SchoolUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectNotes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    SubjectId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_SchoolUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentsSubmitted",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    ObtainableScore = table.Column<long>(nullable: false),
                    ScoreObtained = table.Column<long>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    MarkStatus = table.Column<long>(nullable: false),
                    DateSubmitted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentsSubmitted", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CampusId",
                table: "Assignments",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ClassGradeId",
                table: "Assignments",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ClassId",
                table: "Assignments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SchoolId",
                table: "Assignments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SessionId",
                table: "Assignments",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SubjectId",
                table: "Assignments",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TeacherId",
                table: "Assignments",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TermId",
                table: "Assignments",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_AssignmentId",
                table: "AssignmentsSubmitted",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_CampusId",
                table: "AssignmentsSubmitted",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_ClassGradeId",
                table: "AssignmentsSubmitted",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_ClassId",
                table: "AssignmentsSubmitted",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_SchoolId",
                table: "AssignmentsSubmitted",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_SessionId",
                table: "AssignmentsSubmitted",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_TermId",
                table: "AssignmentsSubmitted",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_CampusId",
                table: "LessonNotes",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_ClassGradeId",
                table: "LessonNotes",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_ClassId",
                table: "LessonNotes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_SchoolId",
                table: "LessonNotes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_SessionId",
                table: "LessonNotes",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_SubjectId",
                table: "LessonNotes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_TeacherId",
                table: "LessonNotes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_TermId",
                table: "LessonNotes",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_CampusId",
                table: "SubjectNotes",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_ClassGradeId",
                table: "SubjectNotes",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_ClassId",
                table: "SubjectNotes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_SchoolId",
                table: "SubjectNotes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_SessionId",
                table: "SubjectNotes",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_SubjectId",
                table: "SubjectNotes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_TeacherId",
                table: "SubjectNotes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_TermId",
                table: "SubjectNotes",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentsSubmitted");

            migrationBuilder.DropTable(
                name: "LessonNotes");

            migrationBuilder.DropTable(
                name: "SubjectNotes");

            migrationBuilder.DropTable(
                name: "Assignments");

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
    }
}

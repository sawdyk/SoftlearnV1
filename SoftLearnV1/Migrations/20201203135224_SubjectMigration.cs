using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class SubjectMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClassGradeId",
                table: "SubjectTeachers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "SchoolSubjects",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MaximumScore",
                table: "SchoolSubjects",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ReportCardOrder",
                table: "SchoolSubjects",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "SubjectCode",
                table: "SchoolSubjects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectDepartment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectDepartment_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectDepartment_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_ClassGradeId",
                table: "SubjectTeachers",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_DepartmentId",
                table: "SchoolSubjects",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectDepartment_CampusId",
                table: "SubjectDepartment",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectDepartment_SchoolId",
                table: "SubjectDepartment",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSubjects_SubjectDepartment_DepartmentId",
                table: "SchoolSubjects",
                column: "DepartmentId",
                principalTable: "SubjectDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeachers_ClassGrades_ClassGradeId",
                table: "SubjectTeachers",
                column: "ClassGradeId",
                principalTable: "ClassGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSubjects_SubjectDepartment_DepartmentId",
                table: "SchoolSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeachers_ClassGrades_ClassGradeId",
                table: "SubjectTeachers");

            migrationBuilder.DropTable(
                name: "SubjectDepartment");

            migrationBuilder.DropIndex(
                name: "IX_SubjectTeachers_ClassGradeId",
                table: "SubjectTeachers");

            migrationBuilder.DropIndex(
                name: "IX_SchoolSubjects_DepartmentId",
                table: "SchoolSubjects");

            migrationBuilder.DropColumn(
                name: "ClassGradeId",
                table: "SubjectTeachers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "SchoolSubjects");

            migrationBuilder.DropColumn(
                name: "MaximumScore",
                table: "SchoolSubjects");

            migrationBuilder.DropColumn(
                name: "ReportCardOrder",
                table: "SchoolSubjects");

            migrationBuilder.DropColumn(
                name: "SubjectCode",
                table: "SchoolSubjects");
        }
    }
}

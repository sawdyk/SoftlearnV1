using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class AlumniMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Students",
                newName: "GenderId");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Parents",
                newName: "GenderId");

            migrationBuilder.CreateTable(
                name: "Alumni",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    GradeTeacherId = table.Column<Guid>(nullable: false),
                    StatusId = table.Column<long>(nullable: false),
                    DateGraduated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alumni_ClassGrades_CampusId",
                        column: x => x.CampusId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_SchoolUsers_GradeTeacherId",
                        column: x => x.GradeTeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassAlumni",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassAlumni", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GenderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ClassAlumni",
                columns: new[] { "Id", "Category" },
                values: new object[,]
                {
                    { 1L, "Alumni" },
                    { 2L, "Class" }
                });

            migrationBuilder.InsertData(
                table: "Gender",
                columns: new[] { "Id", "GenderName" },
                values: new object[,]
                {
                    { 1L, "Male" },
                    { 2L, "Female" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_GenderId",
                table: "Students",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_GenderId",
                table: "Parents",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_CampusId",
                table: "Alumni",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_ClassId",
                table: "Alumni",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_GradeTeacherId",
                table: "Alumni",
                column: "GradeTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_SchoolId",
                table: "Alumni",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_SessionId",
                table: "Alumni",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_StudentId",
                table: "Alumni",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Gender_GenderId",
                table: "Parents",
                column: "GenderId",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Gender_GenderId",
                table: "Students",
                column: "GenderId",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Gender_GenderId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Gender_GenderId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Alumni");

            migrationBuilder.DropTable(
                name: "ClassAlumni");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropIndex(
                name: "IX_Students_GenderId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Parents_GenderId",
                table: "Parents");

            migrationBuilder.RenameColumn(
                name: "GenderId",
                table: "Students",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "GenderId",
                table: "Parents",
                newName: "Gender");
        }
    }
}

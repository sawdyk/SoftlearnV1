using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class StudentDuplicates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentDuplicates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NewStudentFullName = table.Column<string>(nullable: true),
                    ExistingStudentId = table.Column<Guid>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDuplicates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDuplicates_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentDuplicates_Students_ExistingStudentId",
                        column: x => x.ExistingStudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentDuplicates_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentDuplicates_CampusId",
                table: "StudentDuplicates",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDuplicates_ExistingStudentId",
                table: "StudentDuplicates",
                column: "ExistingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDuplicates_SchoolId",
                table: "StudentDuplicates",
                column: "SchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentDuplicates");
        }
    }
}

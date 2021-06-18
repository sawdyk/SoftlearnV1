using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class gradeStudentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasGraduated",
                table: "GradeStudents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "SessionId",
                table: "GradeStudents",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_SessionId",
                table: "GradeStudents",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeStudents_Sessions_SessionId",
                table: "GradeStudents",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeStudents_Sessions_SessionId",
                table: "GradeStudents");

            migrationBuilder.DropIndex(
                name: "IX_GradeStudents_SessionId",
                table: "GradeStudents");

            migrationBuilder.DropColumn(
                name: "HasGraduated",
                table: "GradeStudents");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "GradeStudents");
        }
    }
}

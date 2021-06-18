using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class updateDBMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ExtraCurricularScores",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ExaminationScores",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ContinousAssessmentScores",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "BehavioralScores",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "MarkObtained",
                table: "ExtraCurricularScores",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "MarkObtained",
                table: "ExaminationScores",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "MarkObtained",
                table: "ContinousAssessmentScores",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "MarkObtained",
                table: "BehavioralScores",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}

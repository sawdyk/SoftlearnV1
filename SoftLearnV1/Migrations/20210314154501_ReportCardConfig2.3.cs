using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "ReportCardPosition",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "ReportCardData",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "ExamScore",
                table: "ReportCardData",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "CAScore",
                table: "ReportCardData",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TotalScore",
                table: "ReportCardPosition",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "TotalScore",
                table: "ReportCardData",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "ExamScore",
                table: "ReportCardData",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "CAScore",
                table: "ReportCardData",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}

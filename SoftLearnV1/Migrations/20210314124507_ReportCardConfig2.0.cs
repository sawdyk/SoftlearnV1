using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PercentageScore",
                table: "ReportCardPosition",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "TotalScoreObtainable",
                table: "ReportCardPosition",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageTotalScore",
                table: "ReportCardData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CA_AverageScore",
                table: "ReportCardData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExamAverageScore",
                table: "ReportCardData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FirstTermTotalScore",
                table: "ReportCardData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SecondTermTotalScore",
                table: "ReportCardData",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "ComputeOverallTotalAverage",
                table: "ReportCardConfiguration",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowComputeOverallTotalAverage",
                table: "ReportCardConfiguration",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageScore",
                table: "ReportCardPosition");

            migrationBuilder.DropColumn(
                name: "TotalScoreObtainable",
                table: "ReportCardPosition");

            migrationBuilder.DropColumn(
                name: "AverageTotalScore",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "CA_AverageScore",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "ExamAverageScore",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "FirstTermTotalScore",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "SecondTermTotalScore",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "ComputeOverallTotalAverage",
                table: "ReportCardConfiguration");

            migrationBuilder.DropColumn(
                name: "ShowComputeOverallTotalAverage",
                table: "ReportCardConfiguration");
        }
    }
}

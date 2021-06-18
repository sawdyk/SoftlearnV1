using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "RefFirstTermScoreCompute",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropColumn(
                name: "RefFirstTermScoreShow",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropColumn(
                name: "RefSecondTermScoreCompute",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropColumn(
                name: "RefSecondTermScoreShow",
                table: "ReportCardConfigurationLegend");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "ReportCardData",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "RefFirstTermScoreCompute",
                table: "ReportCardConfigurationLegend",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RefFirstTermScoreShow",
                table: "ReportCardConfigurationLegend",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RefSecondTermScoreCompute",
                table: "ReportCardConfigurationLegend",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RefSecondTermScoreShow",
                table: "ReportCardConfigurationLegend",
                nullable: false,
                defaultValue: false);
        }
    }
}

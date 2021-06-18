using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CA_AverageScore",
                table: "ReportCardData",
                newName: "CumulativeCA_Score");

            migrationBuilder.RenameColumn(
                name: "ComputeCA_Average",
                table: "ReportCardConfiguration",
                newName: "ShowCA_Cumulative");

            migrationBuilder.AddColumn<bool>(
                name: "ComputeCA_Cumulative",
                table: "ReportCardConfiguration",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComputeCA_Cumulative",
                table: "ReportCardConfiguration");

            migrationBuilder.RenameColumn(
                name: "CumulativeCA_Score",
                table: "ReportCardData",
                newName: "CA_AverageScore");

            migrationBuilder.RenameColumn(
                name: "ShowCA_Cumulative",
                table: "ReportCardConfiguration",
                newName: "ComputeCA_Average");
        }
    }
}

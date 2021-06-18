using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamAverageScore",
                table: "ReportCardData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExamAverageScore",
                table: "ReportCardData",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

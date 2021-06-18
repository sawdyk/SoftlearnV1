using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "ReportCardConfigurationLegend",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationLegend_StatusId",
                table: "ReportCardConfigurationLegend",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportCardConfigurationLegend_ActiveInActiveStatus_StatusId",
                table: "ReportCardConfigurationLegend",
                column: "StatusId",
                principalTable: "ActiveInActiveStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportCardConfigurationLegend_ActiveInActiveStatus_StatusId",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropIndex(
                name: "IX_ReportCardConfigurationLegend_StatusId",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ReportCardConfigurationLegend",
                nullable: false,
                defaultValue: false);
        }
    }
}

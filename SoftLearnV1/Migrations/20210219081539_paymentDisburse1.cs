using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class paymentDisburse1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankCode",
                table: "FacilitatorAccountDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "FacilitatorAccountDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RecipientCode",
                table: "FacilitatorAccountDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSettled",
                table: "CourseRefund",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "LearnerAccountDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    RecipientCode = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnerAccountDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnerAccountDetails_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearnerAccountDetails_LearnerId",
                table: "LearnerAccountDetails",
                column: "LearnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearnerAccountDetails");

            migrationBuilder.DropColumn(
                name: "BankCode",
                table: "FacilitatorAccountDetails");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "FacilitatorAccountDetails");

            migrationBuilder.DropColumn(
                name: "RecipientCode",
                table: "FacilitatorAccountDetails");

            migrationBuilder.DropColumn(
                name: "DateSettled",
                table: "CourseRefund");
        }
    }
}

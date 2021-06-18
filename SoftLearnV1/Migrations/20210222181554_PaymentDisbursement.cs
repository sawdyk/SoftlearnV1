using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class PaymentDisbursement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacilitatorPaymentDisbursements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Integration = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Recipient = table.Column<long>(nullable: false),
                    DataStatus = table.Column<string>(nullable: true),
                    TransferCode = table.Column<string>(nullable: true),
                    DataId = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorPaymentDisbursements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilitatorPaymentDisbursements_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearnersPaymentDisbursements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Integration = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Recipient = table.Column<long>(nullable: false),
                    DataStatus = table.Column<string>(nullable: true),
                    TransferCode = table.Column<string>(nullable: true),
                    DataId = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnersPaymentDisbursements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnersPaymentDisbursements_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilitatorPaymentDisbursements_FacilitatorId",
                table: "FacilitatorPaymentDisbursements",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnersPaymentDisbursements_LearnerId",
                table: "LearnersPaymentDisbursements",
                column: "LearnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilitatorPaymentDisbursements");

            migrationBuilder.DropTable(
                name: "LearnersPaymentDisbursements");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class InvoiceAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "InvoiceList",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceTotalId",
                table: "InvoiceList",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_InvoiceTotalId",
                table: "InvoiceList",
                column: "InvoiceTotalId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceList_InvoiceTotal_InvoiceTotalId",
                table: "InvoiceList",
                column: "InvoiceTotalId",
                principalTable: "InvoiceTotal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceList_InvoiceTotal_InvoiceTotalId",
                table: "InvoiceList");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceList_InvoiceTotalId",
                table: "InvoiceList");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "InvoiceList");

            migrationBuilder.DropColumn(
                name: "InvoiceTotalId",
                table: "InvoiceList");
        }
    }
}

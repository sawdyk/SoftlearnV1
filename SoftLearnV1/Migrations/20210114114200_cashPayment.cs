using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class cashPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MethodName",
                table: "PaymentMethods",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.InsertData(
                table: "SchoolPaymentMethods",
                columns: new[] { "Id", "MethodName" },
                values: new object[] { 4L, "Cash Payment" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SchoolPaymentMethods",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.AlterColumn<long>(
                name: "MethodName",
                table: "PaymentMethods",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

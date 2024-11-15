using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class S3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanID",
                keyValue: 1,
                column: "UserID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanID",
                keyValue: 2,
                column: "UserID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "UserID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 2,
                column: "UserID",
                value: 2);

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "OrderDate", "ShippingMethod", "Status", "TotalPrice", "UserID" },
                values: new object[] { 3, new DateTime(2024, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Express", "Processing", 0.0, 2 });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "BookID", "OrderID", "Price", "Quantity" },
                values: new object[] { 3, 3, 22.399999999999999, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "BookID", "OrderID" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanID",
                keyValue: 1,
                column: "UserID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "LoanID",
                keyValue: 2,
                column: "UserID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1,
                column: "UserID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 2,
                column: "UserID",
                value: 1);
        }
    }
}

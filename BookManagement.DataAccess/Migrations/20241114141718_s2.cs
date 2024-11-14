using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class s2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: 1,
                column: "Title",
                value: "7 vien ngoc rong");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: 2,
                column: "Title",
                value: "Batman");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookID", "AuthorID", "BookImages", "BookPDFLink", "CategoryID", "Description", "DiscountID", "Language", "Pages", "Price", "PublishDate", "Quantity", "Title" },
                values: new object[,]
                {
                    { 3, 1, "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg", null, 1, "Winner of the 2024 Hawthornden Prize\nShortlisted for the 2024 Orwell Prize for Political Fiction\nShortlisted for the 2024 Ursula K. Le Guin Prize for Fiction\n\nA singular new novel from Betty Trask Prize-winner Samantha Harvey, Orbital is an eloquent meditation on space and life on our planet through the eyes of six astronauts circling the earth in 24 hours\n\n\"Ravishingly beautiful.\" — Joshua Ferris, New York Times\n\nA slender novel of epic power and the winner of the Booker Prize 2024, Orbital deftly snapshots one day in the lives of six women and men traveling through space. Selected for one of the last space station missions of its kind before the program is dismantled, these astronauts and cosmonauts—from America, Russia, Italy, Britain, and Japan—have left their lives behind to travel at a speed of over seventeen thousand miles an hour as the earth reels below. We glimpse moments of their earthly lives through brief communications with family, their photos and talismans; we watch them whip up dehydrated meals, float in gravity-free sleep, and exercise in regimented routines to prevent atrophying muscles; we witness them form bonds that will stand between them and utter solitude. Most of all, we are with them as they behold and record their silent blue planet. Their experiences of sixteen sunrises and sunsets and the bright, blinking constellations of the galaxy are at once breathtakingly awesome and surprisingly intimate.\n\nProfound and contemplative, Orbital is a moving elegy to our environment and planet.", null, "English", 123, 12.4, new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Doraemon" },
                    { 4, 1, "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg", null, 1, "Alone in space, years from rescue, everyone she knows has vanished.\nOn a colonial mission into uncharted space, Dr. Beth Adler awakens to find her ship ravaged and abandoned. The last thing she recalls is an alarm repeating the same horrifying message. “Quarantine breach.”", null, "English", 123, 12.4, new DateTime(2024, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Pikachu" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "LoanID", "BookID", "Bookmark", "BorrowDate", "DueDate", "FineAmount", "ReturnDate", "Status", "UserID" },
                values: new object[,]
                {
                    { 1, 1, 0, new DateTime(2024, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borrowed", 1 },
                    { 2, 2, 0, new DateTime(2024, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, new DateTime(2024, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borrowed", 1 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "OrderDate", "ShippingMethod", "Status", "TotalPrice", "UserID" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Express", "Completed", 0.0, 1 },
                    { 2, new DateTime(2024, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Express", "Completed", 0.0, 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "BookID", "OrderID", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 12.4, 2 },
                    { 2, 1, 28.399999999999999, 2 },
                    { 3, 2, 7.4000000000000004, 1 },
                    { 4, 2, 36.399999999999999, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Loans",
                keyColumn: "LoanID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Loans",
                keyColumn: "LoanID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "BookID", "OrderID" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "BookID", "OrderID" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "BookID", "OrderID" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "BookID", "OrderID" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderID",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: 1,
                column: "Title",
                value: "Doraemon");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: 2,
                column: "Title",
                value: "Pikachu");
        }
    }
}

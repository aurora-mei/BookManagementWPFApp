using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Discounts_DiscountID",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "authorName",
                table: "Authors",
                newName: "AuthorName");

            migrationBuilder.RenameColumn(
                name: "authorImageURL",
                table: "Authors",
                newName: "AuthorImageURL");

            migrationBuilder.RenameColumn(
                name: "authorEmail",
                table: "Authors",
                newName: "AuthorEmail");

            migrationBuilder.RenameColumn(
                name: "authorDOB",
                table: "Authors",
                newName: "AuthorDOB");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Dob",
                table: "Users",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountID",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "BookPDFLink",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorID", "AuthorDOB", "AuthorEmail", "AuthorImageURL", "AuthorName" },
                values: new object[,]
                {
                    { 1, null, "landa@gmail.com", "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg", "Landa" },
                    { 2, null, "alex@gmail.com", "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg", "Alexandra" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Chidren books" },
                    { 2, "Philosophy" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Address", "Dob", "Email", "Password", "PhoneNumber", "Role", "Status", "Username" },
                values: new object[,]
                {
                    { 1, null, null, "admin@admin.com", "123", null, "Admin", "Active", "admin" },
                    { 2, null, null, "alex@admin.com", "123", null, "User", "Active", "alexandra" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookID", "AuthorID", "BookImages", "BookPDFLink", "CategoryID", "Description", "DiscountID", "Language", "Pages", "Price", "PublishDate", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, 1, "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg", null, 1, "Winner of the 2024 Hawthornden Prize\nShortlisted for the 2024 Orwell Prize for Political Fiction\nShortlisted for the 2024 Ursula K. Le Guin Prize for Fiction\n\nA singular new novel from Betty Trask Prize-winner Samantha Harvey, Orbital is an eloquent meditation on space and life on our planet through the eyes of six astronauts circling the earth in 24 hours\n\n\"Ravishingly beautiful.\" — Joshua Ferris, New York Times\n\nA slender novel of epic power and the winner of the Booker Prize 2024, Orbital deftly snapshots one day in the lives of six women and men traveling through space. Selected for one of the last space station missions of its kind before the program is dismantled, these astronauts and cosmonauts—from America, Russia, Italy, Britain, and Japan—have left their lives behind to travel at a speed of over seventeen thousand miles an hour as the earth reels below. We glimpse moments of their earthly lives through brief communications with family, their photos and talismans; we watch them whip up dehydrated meals, float in gravity-free sleep, and exercise in regimented routines to prevent atrophying muscles; we witness them form bonds that will stand between them and utter solitude. Most of all, we are with them as they behold and record their silent blue planet. Their experiences of sixteen sunrises and sunsets and the bright, blinking constellations of the galaxy are at once breathtakingly awesome and surprisingly intimate.\n\nProfound and contemplative, Orbital is a moving elegy to our environment and planet.", null, "English", 123, 12.4, new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Doraemon" },
                    { 2, 1, "https://i.pinimg.com/736x/99/63/4f/99634f142c41939386fbabd411459929.jpg", null, 1, "Alone in space, years from rescue, everyone she knows has vanished.\nOn a colonial mission into uncharted space, Dr. Beth Adler awakens to find her ship ravaged and abandoned. The last thing she recalls is an alarm repeating the same horrifying message. “Quarantine breach.”", null, "English", 123, 12.4, new DateTime(2024, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Pikachu" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Discounts_DiscountID",
                table: "Books",
                column: "DiscountID",
                principalTable: "Discounts",
                principalColumn: "DiscountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Discounts_DiscountID",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryID",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "Authors",
                newName: "authorName");

            migrationBuilder.RenameColumn(
                name: "AuthorImageURL",
                table: "Authors",
                newName: "authorImageURL");

            migrationBuilder.RenameColumn(
                name: "AuthorEmail",
                table: "Authors",
                newName: "authorEmail");

            migrationBuilder.RenameColumn(
                name: "AuthorDOB",
                table: "Authors",
                newName: "authorDOB");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Dob",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Language",
                table: "Books",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountID",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookPDFLink",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Discounts_DiscountID",
                table: "Books",
                column: "DiscountID",
                principalTable: "Discounts",
                principalColumn: "DiscountID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

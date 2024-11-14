using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changestatusname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Users",
                newName: "UserStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserStatus",
                table: "Users",
                newName: "Status");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceDs.Migrations
{
    /// <inheritdoc />
    public partial class Enabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClosedCart",
                table: "Carts",
                newName: "Enabled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Enabled",
                table: "Carts",
                newName: "ClosedCart");
        }
    }
}

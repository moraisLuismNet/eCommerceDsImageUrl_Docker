using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceDs.Migrations
{
    /// <inheritdoc />
    public partial class dbContextUser3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserEmail",
                table: "Carts");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserEmail",
                table: "Carts",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserEmail",
                table: "Carts");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserEmail",
                table: "Carts",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECOMMERCE2.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedOut",
                table: "Carts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCheckedOut",
                table: "Carts");
        }
    }
}

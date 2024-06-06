using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECOMMERCE2.Migrations
{
    /// <inheritdoc />
    public partial class CardAdressBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCardSaved",
                table: "BillingCard",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAddressSaved",
                table: "BillingAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCardSaved",
                table: "BillingCard");

            migrationBuilder.DropColumn(
                name: "IsAddressSaved",
                table: "BillingAddresses");
        }
    }
}

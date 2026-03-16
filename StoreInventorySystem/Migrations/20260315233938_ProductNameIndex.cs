using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreInventorySystem.Migrations
{
    /// <inheritdoc />
    public partial class ProductNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");
        }
    }
}

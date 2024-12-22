using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp1.Migrations
{
    /// <inheritdoc />
    public partial class СhangingRelationshipsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Warehouses_Warehouseid",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Warehouseid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Warehouseid",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductsWarehouses",
                columns: table => new
                {
                    productsListid = table.Column<int>(type: "integer", nullable: false),
                    warehouseListid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsWarehouses", x => new { x.productsListid, x.warehouseListid });
                    table.ForeignKey(
                        name: "FK_ProductsWarehouses_Products_productsListid",
                        column: x => x.productsListid,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsWarehouses_Warehouses_warehouseListid",
                        column: x => x.warehouseListid,
                        principalTable: "Warehouses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsWarehouses_warehouseListid",
                table: "ProductsWarehouses",
                column: "warehouseListid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsWarehouses");

            migrationBuilder.AddColumn<int>(
                name: "Warehouseid",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Warehouseid",
                table: "Products",
                column: "Warehouseid");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Warehouses_Warehouseid",
                table: "Products",
                column: "Warehouseid",
                principalTable: "Warehouses",
                principalColumn: "id");
        }
    }
}

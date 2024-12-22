using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp1.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStruktTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsWarehouses_Products_productsListid",
                table: "ProductsWarehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsWarehouses_Warehouses_warehouseListid",
                table: "ProductsWarehouses");

            migrationBuilder.DropColumn(
                name: "count",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "warehouseListid",
                table: "ProductsWarehouses",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "productsListid",
                table: "ProductsWarehouses",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsWarehouses_warehouseListid",
                table: "ProductsWarehouses",
                newName: "IX_ProductsWarehouses_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "ProductCount",
                table: "ProductsWarehouses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsWarehouses_Products_ProductId",
                table: "ProductsWarehouses",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsWarehouses_Warehouses_WarehouseId",
                table: "ProductsWarehouses",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsWarehouses_Products_ProductId",
                table: "ProductsWarehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsWarehouses_Warehouses_WarehouseId",
                table: "ProductsWarehouses");

            migrationBuilder.DropColumn(
                name: "ProductCount",
                table: "ProductsWarehouses");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductsWarehouses",
                newName: "warehouseListid");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "ProductsWarehouses",
                newName: "productsListid");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsWarehouses_ProductId",
                table: "ProductsWarehouses",
                newName: "IX_ProductsWarehouses_warehouseListid");

            migrationBuilder.AddColumn<int>(
                name: "count",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsWarehouses_Products_productsListid",
                table: "ProductsWarehouses",
                column: "productsListid",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsWarehouses_Warehouses_warehouseListid",
                table: "ProductsWarehouses",
                column: "warehouseListid",
                principalTable: "Warehouses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

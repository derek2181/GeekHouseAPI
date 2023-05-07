using Microsoft.EntityFrameworkCore.Migrations;

namespace GeeekHouseAPI.Migrations
{
    public partial class subcategoriesAddedProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryProduct");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Subcategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "name",
                value: "Videojuegos");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "name",
                value: "Consolas");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategory_ProductId",
                table: "Subcategory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategory_Product_ProductId",
                table: "Subcategory",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Subcategory_Product_ProductId",
                table: "Subcategory");

            migrationBuilder.DropIndex(
                name: "IX_Subcategory_ProductId",
                table: "Subcategory");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Subcategory");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Product");

            migrationBuilder.CreateTable(
                name: "CategoryProduct",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProduct", x => new { x.CategoriesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Product_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "name",
                value: "Videogame");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "name",
                value: "Console");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProduct_ProductsId",
                table: "CategoryProduct",
                column: "ProductsId");
        }
    }
}

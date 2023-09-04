using Microsoft.EntityFrameworkCore.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

namespace GeeekHouseAPI.Migrations
{
    public partial class ONesubcategoryperproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSubcategory");

            migrationBuilder.AddColumn<int>(
                name: "SubcategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SubcategoryId",
                table: "Product",
                column: "SubcategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Subcategory_SubcategoryId",
                table: "Product",
                column: "SubcategoryId",
                principalTable: "Subcategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Subcategory_SubcategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SubcategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SubcategoryId",
                table: "Product");

            migrationBuilder.CreateTable(
                name: "ProductSubcategory",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    SubcategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubcategory", x => new { x.ProductsId, x.SubcategoriesId });
                    table.ForeignKey(
                        name: "FK_ProductSubcategory_Product_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSubcategory_Subcategory_SubcategoriesId",
                        column: x => x.SubcategoriesId,
                        principalTable: "Subcategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubcategory_SubcategoriesId",
                table: "ProductSubcategory",
                column: "SubcategoriesId");
        }
    }
}

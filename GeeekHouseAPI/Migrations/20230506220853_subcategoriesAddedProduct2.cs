using Microsoft.EntityFrameworkCore.Migrations;

namespace GeeekHouseAPI.Migrations
{
    public partial class subcategoriesAddedProduct2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subcategory_Category_categoryId",
                table: "Subcategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Subcategory_Product_ProductId",
                table: "Subcategory");

            migrationBuilder.DropIndex(
                name: "IX_Subcategory_ProductId",
                table: "Subcategory");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Subcategory");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Subcategory",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Subcategory",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Subcategory_categoryId",
                table: "Subcategory",
                newName: "IX_Subcategory_CategoryId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Category",
                newName: "Name");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategory_Category_CategoryId",
                table: "Subcategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subcategory_Category_CategoryId",
                table: "Subcategory");

            migrationBuilder.DropTable(
                name: "ProductSubcategory");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Subcategory",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Subcategory",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Subcategory_CategoryId",
                table: "Subcategory",
                newName: "IX_Subcategory_categoryId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "name");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Subcategory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subcategory_ProductId",
                table: "Subcategory",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategory_Category_categoryId",
                table: "Subcategory",
                column: "categoryId",
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
    }
}

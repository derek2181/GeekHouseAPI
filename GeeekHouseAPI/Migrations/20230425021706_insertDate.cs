using Microsoft.EntityFrameworkCore.Migrations;

namespace GeeekHouseAPI.Migrations
{
    public partial class insertDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Product_ProductId",
                table: "Image");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Image",
                newName: "productId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_ProductId",
                table: "Image",
                newName: "IX_Image_productId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Product_productId",
                table: "Image",
                column: "productId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Product_productId",
                table: "Image");

            migrationBuilder.RenameColumn(
                name: "productId",
                table: "Image",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_productId",
                table: "Image",
                newName: "IX_Image_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Product_ProductId",
                table: "Image",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

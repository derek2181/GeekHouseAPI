using Microsoft.EntityFrameworkCore.Migrations;

namespace GeeekHouseAPI.Migrations
{
    public partial class S3AmazonObjectName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "objectName",
                table: "Image",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "PREVENTA");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "objectName",
                table: "Image");

            migrationBuilder.UpdateData(
                table: "Availability",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "PREORDEN");
        }
    }
}

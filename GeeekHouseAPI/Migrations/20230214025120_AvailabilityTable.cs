using Microsoft.EntityFrameworkCore.Migrations;

namespace GeeekHouseAPI.Migrations
{
    public partial class AvailabilityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Availability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availability", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Availability",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "DISPONIBLE" });

            migrationBuilder.InsertData(
                table: "Availability",
                columns: new[] { "Id", "Description" },
                values: new object[] { 2, "PREORDEN" });

            migrationBuilder.InsertData(
                table: "Availability",
                columns: new[] { "Id", "Description" },
                values: new object[] { 3, "AGOTADO" });

            migrationBuilder.CreateIndex(
                name: "IX_Product_AvailabilityId",
                table: "Product",
                column: "AvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Availability_AvailabilityId",
                table: "Product",
                column: "AvailabilityId",
                principalTable: "Availability",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Availability_AvailabilityId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Availability");

            migrationBuilder.DropIndex(
                name: "IX_Product_AvailabilityId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "Product");
        }
    }
}

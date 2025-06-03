using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtMart.Migrations
{
    /// <inheritdoc />
    public partial class CreateBid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f19a919-31ca-4d2f-902d-8aabd71dc339");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92fcf00f-5cf8-46e6-8623-f3e84fdf8029");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb731c4b-8801-46c4-8e70-671378d01cb5");

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlacedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43ede431-dc96-466a-94c3-3a5526074ae2", null, "Admin", "ADMIN" },
                    { "55d48390-8c7c-4ad0-aed4-fd1a85e7c2c9", null, "Customer", "CUSTOMER" },
                    { "d9c105b0-80eb-4e02-a37e-8d5398b6abd1", null, "Artist", "ARTIST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ProductId",
                table: "Bids",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43ede431-dc96-466a-94c3-3a5526074ae2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55d48390-8c7c-4ad0-aed4-fd1a85e7c2c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9c105b0-80eb-4e02-a37e-8d5398b6abd1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8f19a919-31ca-4d2f-902d-8aabd71dc339", null, "Customer", "CUSTOMER" },
                    { "92fcf00f-5cf8-46e6-8623-f3e84fdf8029", null, "Admin", "ADMIN" },
                    { "eb731c4b-8801-46c4-8e70-671378d01cb5", null, "Artist", "ARTIST" }
                });
        }
    }
}

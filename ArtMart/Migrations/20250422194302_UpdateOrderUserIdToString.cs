using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtMart.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderUserIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_SellerId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SellerId1",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ccb36cc-075d-46bc-a97c-3c068d681172");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ebf952c1-5173-469e-9000-de5c1f9e8913");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6aa943b-7496-41d7-b08a-3ba508e2f471");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SellerId1",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04ec48dc-4039-47ba-8cfc-ccc310c8922a", null, "Artist", "ARTIST" },
                    { "40a3911a-e1fc-4bf0-803c-ee80a0f54585", null, "Customer", "CUSTOMER" },
                    { "cdd4027a-1ac5-48f7-a827-c8b4f3331b65", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId",
                table: "Orders",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_SellerId",
                table: "Orders",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_SellerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SellerId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04ec48dc-4039-47ba-8cfc-ccc310c8922a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40a3911a-e1fc-4bf0-803c-ee80a0f54585");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cdd4027a-1ac5-48f7-a827-c8b4f3331b65");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerId1",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9ccb36cc-075d-46bc-a97c-3c068d681172", null, "Customer", "CUSTOMER" },
                    { "ebf952c1-5173-469e-9000-de5c1f9e8913", null, "Admin", "ADMIN" },
                    { "f6aa943b-7496-41d7-b08a-3ba508e2f471", null, "Artist", "ARTIST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId1",
                table: "Orders",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId1",
                table: "Orders",
                column: "SellerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId1",
                table: "Orders",
                column: "CustomerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_SellerId1",
                table: "Orders",
                column: "SellerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

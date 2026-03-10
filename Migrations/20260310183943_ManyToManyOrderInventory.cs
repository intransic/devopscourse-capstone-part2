using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogiTrack.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyOrderInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Orders_OrderId",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_OrderId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "InventoryItems");

            migrationBuilder.CreateTable(
                name: "InventoryItemOrder",
                columns: table => new
                {
                    ItemsItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrdersOrderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemOrder", x => new { x.ItemsItemId, x.OrdersOrderId });
                    table.ForeignKey(
                        name: "FK_InventoryItemOrder_InventoryItems_ItemsItemId",
                        column: x => x.ItemsItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryItemOrder_Orders_OrdersOrderId",
                        column: x => x.OrdersOrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemOrder_OrdersOrderId",
                table: "InventoryItemOrder",
                column: "OrdersOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItemOrder");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "InventoryItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_OrderId",
                table: "InventoryItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Orders_OrderId",
                table: "InventoryItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }
    }
}

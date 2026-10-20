using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTools.Migrations
{
    /// <inheritdoc />
    public partial class EventSellAllocationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSellAllocation_BuyEvents_EventBuyId",
                table: "EventSellAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_EventSellAllocation_SellEvents_EventSellId",
                table: "EventSellAllocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventSellAllocation",
                table: "EventSellAllocation");

            migrationBuilder.RenameTable(
                name: "EventSellAllocation",
                newName: "EventSellAllocations");

            migrationBuilder.RenameIndex(
                name: "IX_EventSellAllocation_EventSellId",
                table: "EventSellAllocations",
                newName: "IX_EventSellAllocations_EventSellId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventSellAllocations",
                table: "EventSellAllocations",
                columns: new[] { "EventBuyId", "EventSellId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventSellAllocations_BuyEvents_EventBuyId",
                table: "EventSellAllocations",
                column: "EventBuyId",
                principalTable: "BuyEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventSellAllocations_SellEvents_EventSellId",
                table: "EventSellAllocations",
                column: "EventSellId",
                principalTable: "SellEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSellAllocations_BuyEvents_EventBuyId",
                table: "EventSellAllocations");

            migrationBuilder.DropForeignKey(
                name: "FK_EventSellAllocations_SellEvents_EventSellId",
                table: "EventSellAllocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventSellAllocations",
                table: "EventSellAllocations");

            migrationBuilder.RenameTable(
                name: "EventSellAllocations",
                newName: "EventSellAllocation");

            migrationBuilder.RenameIndex(
                name: "IX_EventSellAllocations_EventSellId",
                table: "EventSellAllocation",
                newName: "IX_EventSellAllocation_EventSellId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventSellAllocation",
                table: "EventSellAllocation",
                columns: new[] { "EventBuyId", "EventSellId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventSellAllocation_BuyEvents_EventBuyId",
                table: "EventSellAllocation",
                column: "EventBuyId",
                principalTable: "BuyEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventSellAllocation_SellEvents_EventSellId",
                table: "EventSellAllocation",
                column: "EventSellId",
                principalTable: "SellEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

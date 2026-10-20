using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTools.Migrations
{
    /// <inheritdoc />
    public partial class AddBrokerToSell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSell_AspNetUsers_UserId",
                table: "EventSell");

            migrationBuilder.DropForeignKey(
                name: "FK_EventSellAllocation_EventSell_EventSellId",
                table: "EventSellAllocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventSell",
                table: "EventSell");

            migrationBuilder.RenameTable(
                name: "EventSell",
                newName: "SellEvents");

            migrationBuilder.RenameIndex(
                name: "IX_EventSell_UserId",
                table: "SellEvents",
                newName: "IX_SellEvents_UserId");

            migrationBuilder.AddColumn<int>(
                name: "BrokerId",
                table: "SellEvents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellEvents",
                table: "SellEvents",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SellEvents_BrokerId",
                table: "SellEvents",
                column: "BrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventSellAllocation_SellEvents_EventSellId",
                table: "EventSellAllocation",
                column: "EventSellId",
                principalTable: "SellEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellEvents_AspNetUsers_UserId",
                table: "SellEvents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellEvents_Brokers_BrokerId",
                table: "SellEvents",
                column: "BrokerId",
                principalTable: "Brokers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventSellAllocation_SellEvents_EventSellId",
                table: "EventSellAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SellEvents_AspNetUsers_UserId",
                table: "SellEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_SellEvents_Brokers_BrokerId",
                table: "SellEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellEvents",
                table: "SellEvents");

            migrationBuilder.DropIndex(
                name: "IX_SellEvents_BrokerId",
                table: "SellEvents");

            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "SellEvents");

            migrationBuilder.RenameTable(
                name: "SellEvents",
                newName: "EventSell");

            migrationBuilder.RenameIndex(
                name: "IX_SellEvents_UserId",
                table: "EventSell",
                newName: "IX_EventSell_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventSell",
                table: "EventSell",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventSell_AspNetUsers_UserId",
                table: "EventSell",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventSellAllocation_EventSell_EventSellId",
                table: "EventSellAllocation",
                column: "EventSellId",
                principalTable: "EventSell",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

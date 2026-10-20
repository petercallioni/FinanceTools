using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTools.Migrations
{
    /// <inheritdoc />
    public partial class AddBuySellEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareHoldings");

            migrationBuilder.CreateTable(
                name: "Brokers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventSell",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    ContractDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AverageSalePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    BrokerageFee = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSell_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuyEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    BrokerId = table.Column<string>(type: "TEXT", nullable: false),
                    BrokerId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    AveragePurchasePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    BrokerageFee = table.Column<decimal>(type: "TEXT", nullable: false),
                    ContractDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyEvents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyEvents_Brokers_BrokerId1",
                        column: x => x.BrokerId1,
                        principalTable: "Brokers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventSellAllocation",
                columns: table => new
                {
                    EventBuyId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventSellId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSellAllocation", x => new { x.EventBuyId, x.EventSellId });
                    table.ForeignKey(
                        name: "FK_EventSellAllocation_BuyEvents_EventBuyId",
                        column: x => x.EventBuyId,
                        principalTable: "BuyEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSellAllocation_EventSell_EventSellId",
                        column: x => x.EventSellId,
                        principalTable: "EventSell",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyEvents_BrokerId1",
                table: "BuyEvents",
                column: "BrokerId1");

            migrationBuilder.CreateIndex(
                name: "IX_BuyEvents_UserId",
                table: "BuyEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSell_UserId",
                table: "EventSell",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSellAllocation_EventSellId",
                table: "EventSellAllocation",
                column: "EventSellId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSellAllocation");

            migrationBuilder.DropTable(
                name: "BuyEvents");

            migrationBuilder.DropTable(
                name: "EventSell");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.CreateTable(
                name: "ShareHoldings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AveragePurchasePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareHoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareHoldings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareHoldings_UserId",
                table: "ShareHoldings",
                column: "UserId");
        }
    }
}

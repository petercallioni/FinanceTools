using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTools.Migrations
{
    /// <inheritdoc />
    public partial class FixBuyEventBrokerForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyEvents_Brokers_BrokerId1",
                table: "BuyEvents");

            migrationBuilder.DropIndex(
                name: "IX_BuyEvents_BrokerId1",
                table: "BuyEvents");

            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "BuyEvents");

            migrationBuilder.RenameColumn(
                name: "BrokerId1",
                table: "BuyEvents",
                newName: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyEvents_BrokerId",
                table: "BuyEvents",
                column: "BrokerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyEvents_Brokers_BrokerId",
                table: "BuyEvents",
                column: "BrokerId",
                principalTable: "Brokers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyEvents_Brokers_BrokerId",
                table: "BuyEvents");

            migrationBuilder.DropIndex(
                name: "IX_BuyEvents_BrokerId",
                table: "BuyEvents");

            migrationBuilder.RenameColumn(
                name: "BrokerId",
                table: "BuyEvents",
                newName: "BrokerId1");

            migrationBuilder.AddColumn<int>(
                name: "BrokerId",
                table: "BuyEvents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BuyEvents_BrokerId1",
                table: "BuyEvents",
                column: "BrokerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyEvents_Brokers_BrokerId1",
                table: "BuyEvents",
                column: "BrokerId1",
                principalTable: "Brokers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using FinanceTools.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTools.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20261020110000_UniqueBrokerName")]
    public partial class UniqueBrokerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "BuyEvents"
                SET "BrokerId" = (
                    SELECT MIN(keeper."Id")
                    FROM "Brokers" AS duplicate
                    JOIN "Brokers" AS keeper ON keeper."Name" = duplicate."Name"
                    WHERE duplicate."Id" = "BuyEvents"."BrokerId")
                WHERE "BrokerId" IN (
                    SELECT duplicate."Id"
                    FROM "Brokers" AS duplicate
                    WHERE duplicate."Id" <> (
                        SELECT MIN(keeper."Id")
                        FROM "Brokers" AS keeper
                        WHERE keeper."Name" = duplicate."Name"));
                """);

            migrationBuilder.Sql("""
                UPDATE "SellEvents"
                SET "BrokerId" = (
                    SELECT MIN(keeper."Id")
                    FROM "Brokers" AS duplicate
                    JOIN "Brokers" AS keeper ON keeper."Name" = duplicate."Name"
                    WHERE duplicate."Id" = "SellEvents"."BrokerId")
                WHERE "BrokerId" IN (
                    SELECT duplicate."Id"
                    FROM "Brokers" AS duplicate
                    WHERE duplicate."Id" <> (
                        SELECT MIN(keeper."Id")
                        FROM "Brokers" AS keeper
                        WHERE keeper."Name" = duplicate."Name"));
                """);

            migrationBuilder.Sql("""
                DELETE FROM "Brokers"
                WHERE "Id" NOT IN (
                    SELECT MIN("Id")
                    FROM "Brokers"
                    GROUP BY "Name");
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Brokers_Name",
                table: "Brokers",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Brokers_Name",
                table: "Brokers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FalconWallet.API.Migrations
{
    public partial class SeedInitialData : Migration
    {
        private static readonly Guid _wallet1Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        private static readonly Guid _wallet2Id = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901");

        private static readonly Guid _user1Id = Guid.Parse("c3d4e5f6-a7b8-9012-cdef-123456789012");
        private static readonly Guid _user2Id = Guid.Parse("d4e5f6a7-b8c9-0123-defa-234567890123");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Currencies
            migrationBuilder.InsertData(
                schema: "wallet",
                table: "Currencies",
                columns: ["Id", "Code", "Name", "ConversionRate", "LastModifyOnUtc"],
                values: new object[,]
                {
                    { 1, "USD", "US Dollar",   1.000000m, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "EUR", "Euro",        0.921000m, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "RUB", "Ruble",      89.500000m, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "TJS", "Somoni",     10.900000m, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                });

            // Wallets
            migrationBuilder.InsertData(
                schema: "wallet",
                table: "Wallets",
                columns: ["Id", "UserId", "Title", "Balance", "CreatedOn", "CurrencyId", "Status"],
                values: new object[,]
                {
                    { _wallet1Id, _user1Id, "Main Wallet",  1500.50m, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, 0 },
                    { _wallet2Id, _user1Id, "Euro Savings",  320.00m, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), 2, 0 },
                    { Guid.Parse("c9d0e1f2-a3b4-5678-cdef-789012345678"), _user2Id, "My Wallet", 5000.00m, new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc), 1, 0 },
                });

            // Transactions
            migrationBuilder.InsertData(
                schema: "wallet",
                table: "Transactions",
                columns: ["Id", "WalletId", "Description", "Amount", "Type", "CreatedOn"],
                values: new object[,]
                {
                    { Guid.NewGuid(), _wallet1Id, "Initial deposit",   1000.00m, 0, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                    { Guid.NewGuid(), _wallet1Id, "Top up",             500.50m, 0, new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc) },
                    { Guid.NewGuid(), _wallet1Id, "Coffee shop",         25.00m, 1, new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { Guid.NewGuid(), _wallet1Id, "Grocery store",       75.00m, 1, new DateTime(2024, 1, 4, 0, 0, 0, DateTimeKind.Utc) },
                    { Guid.NewGuid(), _wallet2Id, "Euro deposit",       320.00m, 0, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(schema: "wallet", table: "Transactions", keyColumn: "WalletId",
                keyValue: _wallet1Id);

            migrationBuilder.DeleteData(schema: "wallet", table: "Transactions", keyColumn: "WalletId",
                keyValue: _wallet2Id);

            migrationBuilder.DeleteData(schema: "wallet", table: "Wallets", keyColumn: "Id",
                keyValue: _wallet1Id);

            migrationBuilder.DeleteData(schema: "wallet", table: "Wallets", keyColumn: "Id",
                keyValue: _wallet2Id);

            migrationBuilder.DeleteData(schema: "wallet", table: "Wallets", keyColumn: "Id",
                keyValue: Guid.Parse("c9d0e1f2-a3b4-5678-cdef-789012345678"));

            migrationBuilder.DeleteData(schema: "wallet", table: "Currencies", keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4 });
        }
    }
}
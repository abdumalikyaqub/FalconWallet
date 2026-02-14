using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FalconWallet.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "wallet");

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "wallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    ConversionRate = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    LastModifyOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                schema: "wallet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Balance = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "wallet",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                schema: "wallet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "wallet",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                schema: "wallet",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WalletId",
                schema: "wallet",
                table: "Transactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CurrencyId",
                schema: "wallet",
                table: "Wallets",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "wallet");

            migrationBuilder.DropTable(
                name: "Wallets",
                schema: "wallet");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "wallet");
        }
    }
}

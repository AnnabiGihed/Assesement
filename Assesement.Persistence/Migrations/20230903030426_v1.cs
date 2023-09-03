using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Assessment.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    BreweryStockId = table.Column<int>(type: "int", nullable: false),
                    AlchoholPercentage = table.Column<float>(type: "real", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beer", x => x.Id);
                    table.UniqueConstraint("AK_Beer_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Brewery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brewery", x => x.Id);
                    table.UniqueConstraint("AK_Brewery_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.UniqueConstraint("AK_Client_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Wholesaler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wholesaler", x => x.Id);
                    table.UniqueConstraint("AK_Wholesaler_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "BreweryStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BeerId = table.Column<int>(type: "int", nullable: false),
                    BreweryId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreweryStock", x => new { x.Id, x.BeerId, x.BreweryId });
                    table.ForeignKey(
                        name: "FK_BreweryStock_Beer_BeerId",
                        column: x => x.BeerId,
                        principalTable: "Beer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreweryStock_Brewery_BreweryId",
                        column: x => x.BreweryId,
                        principalTable: "Brewery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BeerId = table.Column<int>(type: "int", nullable: false),
                    BreweryId = table.Column<int>(type: "int", nullable: false),
                    WholesalerId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => new { x.Id, x.BeerId, x.BreweryId, x.WholesalerId });
                    table.ForeignKey(
                        name: "FK_Transaction_Beer_BeerId",
                        column: x => x.BeerId,
                        principalTable: "Beer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Brewery_BreweryId",
                        column: x => x.BreweryId,
                        principalTable: "Brewery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Wholesaler_WholesalerId",
                        column: x => x.WholesalerId,
                        principalTable: "Wholesaler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WholesalerSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BeerId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    WholesalerId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitePrice = table.Column<float>(type: "real", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholesalerSales", x => new { x.Id, x.WholesalerId, x.BeerId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_WholesalerSales_Beer_BeerId",
                        column: x => x.BeerId,
                        principalTable: "Beer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WholesalerSales_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WholesalerSales_Wholesaler_WholesalerId",
                        column: x => x.WholesalerId,
                        principalTable: "Wholesaler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WholesalerStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WholesalerId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    BeerId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WholesalerStock", x => new { x.Id, x.WholesalerId });
                    table.ForeignKey(
                        name: "FK_WholesalerStock_Beer_BeerId",
                        column: x => x.BeerId,
                        principalTable: "Beer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WholesalerStock_Wholesaler_WholesalerId",
                        column: x => x.WholesalerId,
                        principalTable: "Wholesaler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Beer",
                columns: new[] { "Id", "AlchoholPercentage", "BreweryStockId", "CreatedDate", "Name", "Price", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 6.6f, 1, null, "Leffe Blonde", 2.2f, null },
                    { 2, 9f, 2, null, "Chimay Bleue (Blue Cap)", 58.38f, null },
                    { 3, 4.8f, 3, null, "Chimay Dorée (Gold)", 2.1f, null },
                    { 4, 7f, 4, null, "Chimay Rouge (Red Cap)", 2.1f, null },
                    { 5, 8f, 5, null, "Chimay Triple (White Cap)", 2.2f, null }
                });

            migrationBuilder.InsertData(
                table: "Brewery",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, "Abbaye de Leffe", null },
                    { 2, null, "Bières de Chimay", null }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { 1, null, "James Peeters", null });

            migrationBuilder.InsertData(
                table: "Wholesaler",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[] { 1, null, "GeneDrinks", null });

            migrationBuilder.InsertData(
                table: "BreweryStock",
                columns: new[] { "BeerId", "BreweryId", "Id", "Count", "CreatedDate", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, 1, 90, null, null },
                    { 2, 2, 2, 50, null, null },
                    { 3, 2, 3, 20, null, null },
                    { 4, 2, 4, 30, null, null },
                    { 5, 2, 5, 50, null, null }
                });

            migrationBuilder.InsertData(
                table: "Transaction",
                columns: new[] { "BeerId", "BreweryId", "Id", "WholesalerId", "CreatedDate", "Quantity", "UpdatedDate" },
                values: new object[] { 1, 1, 1, 1, null, 10, null });

            migrationBuilder.InsertData(
                table: "WholesalerStock",
                columns: new[] { "Id", "WholesalerId", "BeerId", "Count", "CreatedDate", "UpdatedDate" },
                values: new object[] { 1, 1, 1, 10, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_BreweryStock_BeerId",
                table: "BreweryStock",
                column: "BeerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BreweryStock_BreweryId",
                table: "BreweryStock",
                column: "BreweryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BeerId",
                table: "Transaction",
                column: "BeerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BreweryId",
                table: "Transaction",
                column: "BreweryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_WholesalerId",
                table: "Transaction",
                column: "WholesalerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalerSales_BeerId",
                table: "WholesalerSales",
                column: "BeerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalerSales_ClientId",
                table: "WholesalerSales",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalerSales_WholesalerId",
                table: "WholesalerSales",
                column: "WholesalerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalerStock_BeerId",
                table: "WholesalerStock",
                column: "BeerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalerStock_WholesalerId",
                table: "WholesalerStock",
                column: "WholesalerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreweryStock");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "WholesalerSales");

            migrationBuilder.DropTable(
                name: "WholesalerStock");

            migrationBuilder.DropTable(
                name: "Brewery");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Beer");

            migrationBuilder.DropTable(
                name: "Wholesaler");
        }
    }
}

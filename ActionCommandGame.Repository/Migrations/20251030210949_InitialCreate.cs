using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActionCommandGame.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Fuel = table.Column<int>(type: "int", nullable: false),
                    Attack = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    ActionCooldownSeconds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NegativeGameEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefenseWithGearDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefenseWithoutGearDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefenseLoss = table.Column<int>(type: "int", nullable: false),
                    Probability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NegativeGameEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PositiveGameEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Money = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    Probability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositiveGameEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    RemainingFuel = table.Column<int>(type: "int", nullable: false),
                    RemainingAttack = table.Column<int>(type: "int", nullable: false),
                    RemainingDefense = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Money = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    LastActionExecutedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentFuelPlayerItemId = table.Column<int>(type: "int", nullable: true),
                    CurrentAttackPlayerItemId = table.Column<int>(type: "int", nullable: true),
                    CurrentDefensePlayerItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_PlayerItems_CurrentAttackPlayerItemId",
                        column: x => x.CurrentAttackPlayerItemId,
                        principalTable: "PlayerItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_PlayerItems_CurrentDefensePlayerItemId",
                        column: x => x.CurrentDefensePlayerItemId,
                        principalTable: "PlayerItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_PlayerItems_CurrentFuelPlayerItemId",
                        column: x => x.CurrentFuelPlayerItemId,
                        principalTable: "PlayerItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_ItemId",
                table: "PlayerItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_PlayerId",
                table: "PlayerItems",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentAttackPlayerItemId",
                table: "Players",
                column: "CurrentAttackPlayerItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentDefensePlayerItemId",
                table: "Players",
                column: "CurrentDefensePlayerItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentFuelPlayerItemId",
                table: "Players",
                column: "CurrentFuelPlayerItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerItems_Players_PlayerId",
                table: "PlayerItems",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerItems_Items_ItemId",
                table: "PlayerItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerItems_Players_PlayerId",
                table: "PlayerItems");

            migrationBuilder.DropTable(
                name: "NegativeGameEvents");

            migrationBuilder.DropTable(
                name: "PositiveGameEvents");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "PlayerItems");
        }
    }
}

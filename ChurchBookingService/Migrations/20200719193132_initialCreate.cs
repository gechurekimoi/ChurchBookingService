using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchBookingService.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChurchDay",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceDate = table.Column<DateTime>(nullable: false),
                    NoOfServices = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceBooked",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChurchDayId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    ServiceNo = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceBooked", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceBooked_ChurchDay_ChurchDayId",
                        column: x => x.ChurchDayId,
                        principalTable: "ChurchDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceBooked_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBooked_ChurchDayId",
                table: "ServiceBooked",
                column: "ChurchDayId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBooked_MemberId",
                table: "ServiceBooked",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceBooked");

            migrationBuilder.DropTable(
                name: "ChurchDay");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchBookingService.Migrations
{
    public partial class permMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SeatNo",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Members");

            migrationBuilder.CreateTable(
                name: "PermanentMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Age = table.Column<string>(nullable: true),
                    Residence = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    SeatNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermanentMember", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermanentMember");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SeatNo",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

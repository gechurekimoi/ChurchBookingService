using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchBookingService.Migrations
{
    public partial class addedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Age",
                table: "Members",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDeadline",
                table: "ChurchDay",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDeadline",
                table: "ChurchDay");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Members",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

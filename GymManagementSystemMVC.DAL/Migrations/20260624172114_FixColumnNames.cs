using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystemMVC.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UbdateAt",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Plans",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Plans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Plans",
                newName: "Created");

            migrationBuilder.AddColumn<DateTime>(
                name: "UbdateAt",
                table: "Plans",
                type: "datetime2",
                nullable: true);
        }
    }
}

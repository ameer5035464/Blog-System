using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSystem.DAL._Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BlockPeriod",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockPeriod",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "AspNetUsers");
        }
    }
}

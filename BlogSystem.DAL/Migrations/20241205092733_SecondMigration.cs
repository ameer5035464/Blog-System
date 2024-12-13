using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Posts",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 12, 5, 7, 31, 29, 410, DateTimeKind.Unspecified).AddTicks(3289), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Posts",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 12, 5, 7, 31, 29, 410, DateTimeKind.Unspecified).AddTicks(2726), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Comments",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 12, 5, 7, 31, 29, 411, DateTimeKind.Unspecified).AddTicks(5115), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Posts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 12, 5, 7, 31, 29, 410, DateTimeKind.Unspecified).AddTicks(3289), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Posts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 12, 5, 7, 31, 29, 410, DateTimeKind.Unspecified).AddTicks(2726), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Comments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 12, 5, 7, 31, 29, 411, DateTimeKind.Unspecified).AddTicks(5115), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");
        }
    }
}

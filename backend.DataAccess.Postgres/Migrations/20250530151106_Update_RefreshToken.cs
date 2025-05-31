using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.DataAccess.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Update_RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenValidityMins",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireIn",
                table: "RefreshTokens",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireIn",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenValidityMins",
                table: "RefreshTokens",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

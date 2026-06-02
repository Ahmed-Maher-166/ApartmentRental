using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentRental.Reporisitory.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialMigrationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StreetName",
                table: "Apartments",
                type: "varchar(35)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Apartments",
                type: "varchar(35)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StreetName",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(35)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(35)");
        }
    }
}

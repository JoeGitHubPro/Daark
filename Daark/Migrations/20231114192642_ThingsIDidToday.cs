using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Daark.Migrations
{
    /// <inheritdoc />
    public partial class ThingsIDidToday : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThingsIDidToday",
                table: "DaarkRealEstate",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThingsIDidToday",
                table: "DaarkRealEstate");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterMealMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedEnumsToGrocery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Storage",
                table: "Groceries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Groceries",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Storage",
                table: "Groceries");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Groceries");
        }
    }
}

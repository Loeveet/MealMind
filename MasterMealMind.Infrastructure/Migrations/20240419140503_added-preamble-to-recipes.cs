using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterMealMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedpreambletorecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Recipes",
                newName: "Preamble");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "Preamble",
                table: "Recipes",
                newName: "Desc");
        }
    }
}

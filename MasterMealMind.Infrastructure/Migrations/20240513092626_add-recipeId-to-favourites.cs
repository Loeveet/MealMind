using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterMealMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addrecipeIdtofavourites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipeId",
                table: "FavouriteRecipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "FavouriteRecipes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterMealMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixrecipeIdinfavouritestoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "FavouriteRecipes",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RecipeId",
                table: "FavouriteRecipes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

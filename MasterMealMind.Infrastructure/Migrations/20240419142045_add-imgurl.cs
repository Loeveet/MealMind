using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterMealMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addimgurl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgURL",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgURL",
                table: "Recipes");
        }
    }
}

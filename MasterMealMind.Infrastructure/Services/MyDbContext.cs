using Microsoft.EntityFrameworkCore;
using MasterMealMind.Core.Models;

namespace MasterMealMind.Infrastructure.Services
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<Grocery> Groceries { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<FavouriteRecipe> FavouriteRecipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Recipe>().ToTable("Recipes");
            //modelBuilder.Entity<FavouriteRecipe>().ToTable("FavouriteRecipes");

			//modelBuilder.Entity<FavouriteRecipe>()
			//	.Property(fr => fr.Id)
			//	.ValueGeneratedOnAdd()
			//	.UseIdentityColumn();
		}

    }
}

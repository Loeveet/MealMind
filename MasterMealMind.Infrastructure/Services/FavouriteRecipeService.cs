using MasterMealMind.Core.Interfaces;
using OpenQA.Selenium;
using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MasterMealMind.Infrastructure.Services
{
	public class FavouriteRecipeService(MyDbContext context) : IFavouriteRecipeService
	{
		private readonly MyDbContext _context = context;

		public async Task AddAsync(int recipeId)
		{
			var recipe = await _context.Recipes.FindAsync(recipeId) ?? throw new NotFoundException();
			var newFavouriteRecipe = new FavouriteRecipe
			{
				Title = recipe.Title,
				Description = recipe.Description,
				Preamble = recipe.Preamble,
				Ingredients = recipe.Ingredients,
				ImgURL = recipe.ImgURL,
				RecipeId = recipeId
			};
			await _context.FavouriteRecipes.AddAsync(newFavouriteRecipe);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> ExistsAsync(int recipeId)
		{
			return await _context.FavouriteRecipes.AnyAsync(x => x.RecipeId == recipeId);
		}

		public async Task<IEnumerable<FavouriteRecipe>> GetAsync()
		{
			var favouriteRecipes = await _context.FavouriteRecipes.ToListAsync() ?? [];
			return favouriteRecipes;
		}

        public async Task<FavouriteRecipe> GetOneAsync(int recipeId)
        {
            var recipe = await _context.FavouriteRecipes.FindAsync(recipeId) ?? throw new NotFoundException();
            return recipe;
        }

        public async Task RemoveAsync(int recipeId)
		{
			var recipe = await _context.FavouriteRecipes.FindAsync(recipeId) ?? throw new NotFoundException();
			_context.FavouriteRecipes.Remove(recipe);
			await _context.SaveChangesAsync();
		}
	}
}

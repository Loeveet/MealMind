using MasterMealMind.Core.Interfaces;
using OpenQA.Selenium;
using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			};
			await _context.FavouriteRecipes.AddAsync(newFavouriteRecipe);
			await _context.SaveChangesAsync();
		}
	}
}

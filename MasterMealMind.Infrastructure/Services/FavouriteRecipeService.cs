using MasterMealMind.Core.Interfaces;
using OpenQA.Selenium;
using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MasterMealMind.Core.Interfaces.IRepositories;

namespace MasterMealMind.Infrastructure.Services
{
	public class FavouriteRecipeService(IFavouriteRecipeRepository favouriteRecipeRepository, IRecipeService recipeService) : IFavouriteRecipeService
	{
		private readonly IFavouriteRecipeRepository _favouriteRecipeRepository = favouriteRecipeRepository;
		private readonly IRecipeService _recipeService = recipeService;

		public async Task AddAsync(int recipeId)
		{
			var recipe = await _recipeService.GetOneAsync(recipeId);
			var newFavouriteRecipe = new FavouriteRecipe
			{
				Title = recipe.Title,
				Description = recipe.Description,
				Preamble = recipe.Preamble,
				Ingredients = recipe.Ingredients,
				ImgURL = recipe.ImgURL,
				RecipeId = recipeId
			};
			await _favouriteRecipeRepository.AddAsync(newFavouriteRecipe);
		}

		public async Task<bool> ExistsAsync(int recipeId) => await _favouriteRecipeRepository.ExistsAsync(recipeId);

		public async Task<IEnumerable<FavouriteRecipe>> GetAsync() => await _favouriteRecipeRepository.GetAsync();

		public async Task<FavouriteRecipe> GetOneAsync(int recipeId) => await _favouriteRecipeRepository.GetOneAsync(recipeId);

        public async Task RemoveAsync(int recipeId) => await _favouriteRecipeRepository.RemoveAsync(await GetOneAsync(recipeId));

		public async Task UpdateAsync(int id, string[] ingredients, string[] description, string title, string preamble)
		{
			var existingRecipe = await GetOneAsync(id) ?? throw new NotFoundException();

			existingRecipe.Title = title;
			existingRecipe.Preamble = preamble;
			existingRecipe.Ingredients = JoinData("|", ingredients);
			existingRecipe.Description = JoinData("|", description);

			await _favouriteRecipeRepository.UpdateAsync(existingRecipe);
		}
		private static string JoinData(string separator, string[] data)
		{
			return string.Join(separator, data);
		}
	}
}

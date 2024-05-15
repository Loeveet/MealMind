using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Interfaces.IRepositories
{
	public interface IFavouriteRecipeRepository
	{
		Task AddAsync(FavouriteRecipe newFavouriteRecipe);
		Task<IEnumerable<FavouriteRecipe>> GetAsync();
		Task RemoveAsync(FavouriteRecipe recipe);
		Task<bool> ExistsAsync(int recipeId);
		Task<FavouriteRecipe> GetOneAsync(int recipeId);
		Task UpdateAsync(FavouriteRecipe recipe);
		//Task<List<string?>> GetTitlesAsync();
		//Task AddAsync(List<Recipe> recipes);
		//Task<IEnumerable<Recipe>> GetTenAsync();
		//Task<IEnumerable<Recipe>> GetBasedOnSearchAsync();
	}
}

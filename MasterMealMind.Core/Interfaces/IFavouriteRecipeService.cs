using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Interfaces
{
	public interface IFavouriteRecipeService
	{
		Task AddAsync(int recipeId);
		Task<IEnumerable<FavouriteRecipe>> GetAsync();
		Task RemoveAsync(int recipeId);
		Task<bool> ExistsAsync(int recipeId);
		Task<FavouriteRecipe> GetOneAsync(int recipeId);
		//Task<List<string?>> GetTitlesAsync();
		//Task AddAsync(List<Recipe> recipes);
		//Task<IEnumerable<Recipe>> GetTenAsync();
		//Task<IEnumerable<Recipe>> GetBasedOnSearchAsync();
	}
}

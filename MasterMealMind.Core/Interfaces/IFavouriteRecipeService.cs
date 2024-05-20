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
		Task UpdateAsync(int id, string[] ingredients, string[] description, string title, string preamble);
	}
}

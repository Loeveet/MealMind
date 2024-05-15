using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Interfaces.IRepositories
{
    public interface IRecipeRepository
    {
		Task<IEnumerable<Recipe>> GetAsync();
		Task<Recipe> GetOneAsync(int recipeId);
		Task<IEnumerable<string?>> GetTitlesAsync();
		Task AddAsync(List<Recipe> recipes);
		Task<IEnumerable<Recipe>> GetTenAsync();
		Task<IEnumerable<Recipe>> GetBasedOnSearchAsync(string[] input);
	}
}

using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Interfaces.IRepositories;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Repositories
{
	public class RecipeRepository(MyDbContext context) : IRecipeRepository
	{
		private readonly MyDbContext _context = context;

		public async Task AddAsync(List<Recipe> recipes)
		{
			await _context.Recipes.AddRangeAsync(recipes);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Recipe>> GetAsync() => await _context.Recipes.ToListAsync() ?? [];

		public async Task<IEnumerable<Recipe>> GetBasedOnSearchAsync(string[] searchWords)
		{
			var recipes = await _context.Recipes
				.Where(recipe => searchWords.All(word =>
					(!string.IsNullOrEmpty(recipe.Ingredients) && recipe.Ingredients.ToLower().Contains(word.ToLower()))
					|| (!string.IsNullOrEmpty(recipe.Title) && recipe.Title.ToLower().Contains(word.ToLower()))))
				.ToListAsync();
			return recipes;
		}


		public async Task<Recipe> GetOneAsync(int recipeId) => await _context.Recipes.FindAsync(recipeId) ?? throw new NotFoundException();


		public async Task<IEnumerable<Recipe>> GetTenAsync() => await _context.Recipes
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync() ?? [];

		public async Task<IEnumerable<string?>> GetTitlesAsync() => await _context.Recipes.Select(x => x.Title).ToListAsync() ?? [];

	}
}

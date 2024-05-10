using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Services
{
	public class RecipeService(MyDbContext context, ISearchService searchService) : IRecipeService
	{
		private readonly MyDbContext _context = context;
		private readonly ISearchService _searchService = searchService;

		public async Task<List<Recipe>> GetAsync()
		{
			var recipes = await _context.Recipes.ToListAsync() ?? [];
			return recipes;
		}
		public async Task AddAsync(List<Recipe> recipes)
		{
			await _context.Recipes.AddRangeAsync(recipes);
			await _context.SaveChangesAsync();
		}

		public async Task<List<string?>> GetTitlesAsync()
		{
			var recipes = await _context.Recipes.Select(x => x.Title).ToListAsync() ?? [];
			return recipes;
		}

        public async Task<IEnumerable<Recipe>> GetTenAsync()
        {
            var randTen = await _context.Recipes
                .OrderBy(x => Guid.NewGuid()) // Slumpa ordningen
                .Take(10)
                .ToListAsync(); 
			
			return randTen;
        }

        public async Task<IEnumerable<Recipe>> GetBasedOnSearchAsync()
		{
			var searchString = _searchService.GetSearchString();
			var searchWords = searchString.Split(' ');
			var recipes = await _context.Recipes
				.Where(recipe => searchWords.All(word => recipe.Ingredients.Contains(word.ToLower())))
				.ToListAsync();
			return recipes;
		}
	}
}

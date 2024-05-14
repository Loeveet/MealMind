using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenQA.Selenium;
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

		public async Task<IEnumerable<Recipe>> GetAsync() => await _context.Recipes.ToListAsync() ?? [];
		public async Task AddAsync(List<Recipe> recipes)
		{
			await _context.Recipes.AddRangeAsync(recipes);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<string?>> GetTitlesAsync() => await _context.Recipes.Select(x => x.Title).ToListAsync() ?? [];

        public async Task<IEnumerable<Recipe>> GetTenAsync() => await _context.Recipes
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync() ?? [];

        public async Task<IEnumerable<Recipe>> GetBasedOnSearchAsync(string input)
		{
			var searchString = _searchService.GetSearchString();
			var searchWords = searchString != string.Empty ? searchString.Split(' ') : input.Split(' ');
			if(searchString == string.Empty)
				_searchService.SetSearchString(input);
			var recipes = await _context.Recipes
				.Where(recipe => searchWords.All(word => recipe.Ingredients.Contains(word.ToLower())))
				.ToListAsync();
			return recipes;
		}

        public async Task<Recipe> GetOneAsync(int recipeId) => await _context.Recipes.FindAsync(recipeId) ?? throw new NotFoundException();
    }
}

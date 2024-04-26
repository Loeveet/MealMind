using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Services
{
	public class RecipeService : IRecipeService
	{
		private readonly MyDbContext _context;

		public RecipeService(MyDbContext context)
		{
			_context = context;
		}
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

		public async Task<List<string>> GetTitlesAsync()
		{
			var recipes = await _context.Recipes.Select(x => x.Title).ToListAsync() ?? [];
			return recipes;
		}
	}
}

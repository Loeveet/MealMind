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
	public class FavouriteRecipeRepository(MyDbContext context) : IFavouriteRecipeRepository
	{
		private readonly MyDbContext _context = context;

		public async Task AddAsync(FavouriteRecipe newFavouriteRecipe)
		{
			await _context.FavouriteRecipes.AddAsync(newFavouriteRecipe);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> ExistsAsync(int recipeId) => await _context.FavouriteRecipes.AnyAsync(x => x.RecipeId == recipeId);

		public async Task<IEnumerable<FavouriteRecipe>> GetAsync() => await _context.FavouriteRecipes.ToListAsync() ?? [];

		public async Task<FavouriteRecipe> GetOneAsync(int recipeId) => await _context.FavouriteRecipes.FindAsync(recipeId) ?? throw new NotFoundException();


		public async Task RemoveAsync(FavouriteRecipe recipe)
		{
			_context.FavouriteRecipes.Remove(recipe);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(FavouriteRecipe recipe)
		{
			_context.FavouriteRecipes.Update(recipe);
			await _context.SaveChangesAsync();
		}
	}
}

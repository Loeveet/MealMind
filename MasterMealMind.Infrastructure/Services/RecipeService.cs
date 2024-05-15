using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Interfaces.IRepositories;
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
	public class RecipeService(IRecipeRepository recipeRepository, ISearchService searchService) : IRecipeService
	{
		private readonly IRecipeRepository _recipeRepository = recipeRepository;
		private readonly ISearchService _searchService = searchService;

		public async Task<IEnumerable<Recipe>> GetAsync() => await _recipeRepository.GetAsync();
		public async Task AddAsync(List<Recipe> recipes) => await _recipeRepository.AddAsync(recipes);

		public async Task<IEnumerable<string?>> GetTitlesAsync() => await _recipeRepository.GetTitlesAsync();

		public async Task<IEnumerable<Recipe>> GetTenAsync() => await _recipeRepository.GetTenAsync();

        public async Task<IEnumerable<Recipe>> GetBasedOnSearchAsync(string input)
		{
			var searchString = _searchService.GetSearchString();
			var searchWords = searchString != string.Empty ? searchString.Split(' ') : input.Split(' ');

			if(searchString == string.Empty)
				_searchService.SetSearchString(input);

			var recipes = await _recipeRepository.GetBasedOnSearchAsync(searchWords);

			return recipes;
		}

        public async Task<Recipe> GetOneAsync(int recipeId) => await _recipeRepository.GetOneAsync(recipeId);
    }
}

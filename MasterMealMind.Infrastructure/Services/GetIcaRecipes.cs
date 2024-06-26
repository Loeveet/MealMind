﻿using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Scraper.Scrapers;
using static System.Net.WebRequestMethods;

namespace MasterMealMind.Infrastructure.Services
{
	public class GetIcaRecipes(IRecipeService recipeService, IICAScraper icaScraper) : IGetIcaRecipes
	{
		private readonly IRecipeService _recipeService = recipeService;
		private readonly IICAScraper _icaScraper = icaScraper;

		public async Task GetIcaAsync()
		{
			var icaEndpoints = new string[]
			{
				"https://www.ica.se/recept/frukost/",
				"https://www.ica.se/recept/vardag/frukost/",
				"https://www.ica.se/recept/",
				"https://www.ica.se/recept/billig",
				"https://www.ica.se/recept/vardag/middag/",
				"https://www.ica.se/recept/mellanmal/",
				"https://www.ica.se/recept/fika/",
				"https://www.ica.se/recept/efterratt/"

			};

			var allRecipes = new List<Recipe>();

			foreach (var endpoint in icaEndpoints)
			{
				try
				{
					var recipes = await _icaScraper.StartScraperAsync(endpoint);
					allRecipes.AddRange(recipes);
				}
				catch
				{
					continue;
				}
			}

			allRecipes = allRecipes.GroupBy(recipe => recipe.Title).Select(group => group.First()).ToList();

			var currentRecipeTitles = await _recipeService.GetTitlesAsync();
			var newRecipes = allRecipes.Where(recipe => !currentRecipeTitles.Contains(recipe.Title)).ToList();


			await _recipeService.AddAsync(newRecipes);

		}

	}

}


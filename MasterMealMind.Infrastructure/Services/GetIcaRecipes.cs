using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Scraper.Scrapers;
using static System.Net.WebRequestMethods;

namespace MasterMealMind.Infrastructure.Services
{
	public class GetIcaRecipes(IRecipeService recipeService) : IGetIcaRecipes
	{
		private readonly IRecipeService _recipeService = recipeService;

		public async Task GetIcaAsync()
		{
			var icaEndpoints = new string[]
			{
				"https://www.ica.se/recept/",
				"https://www.ica.se/recept/billig",
				"https://www.ica.se/recept/frukost/",				
				"https://www.ica.se/recept/vardag/frukost/",
				"https://www.ica.se/recept/vardag/middag/",
				"https://www.ica.se/recept/mellanmal/",
				"https://www.ica.se/recept/fika/",
				"https://www.ica.se/recept/efterratt/"

			};

			var allRecipes = new List<Recipe>(); 
			var uniqueRecipeTitles = new HashSet<string>();

			foreach (var endpoint in icaEndpoints)
			{
				var icaScraper = new ICAscraper();
				var recipes = await icaScraper.GetAsync(endpoint);
				allRecipes.AddRange(recipes); 

				foreach (var recipe in recipes)
				{
					uniqueRecipeTitles.Add(recipe.Title);
				}
			}

			allRecipes = allRecipes.GroupBy(recipe => recipe.Title).Select(group => group.First()).ToList();

			var currentRecipeTitles = await _recipeService.GetTitlesAsync();
			var newRecipes = allRecipes.Where(recipe => !currentRecipeTitles.Contains(recipe.Title)).ToList();


			await _recipeService.AddAsync(newRecipes);
		}

	}

}


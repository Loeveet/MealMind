using MasterMealMind.Core.Interfaces;
using MasterMealMind.Scraper.Scrapers;

namespace MasterMealMind.Infrastructure.Services
{
	public class GetIcaRecipies(IRecipeService recipeService) : IGetIcaRecipies
    {
		private readonly IRecipeService _recipeService = recipeService;

		public async Task GetIcaAsync()
		{
			var icaScraper = new ICAscraper();
			var recipes = await icaScraper.GetAsync();
			var currentRecipeTitles = await _recipeService.GetTitlesAsync();
			var newRecipes = recipes.Where(x => !currentRecipeTitles.Contains(x.Title)).ToList();
			await _recipeService.AddAsync(newRecipes);

		}

	}

}


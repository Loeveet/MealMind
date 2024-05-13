using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterMealMind.Web.Pages
{
    public class IndexModel(ISearchService searchService, 
        IRecipeService recipeService, 
        IGetIcaRecipies getIcaRecipies,
        IFavouriteRecipeService favouriteRecipeService) : PageModel
    {
		private readonly ISearchService _searchService = searchService;
		private readonly IRecipeService _recipeService = recipeService;
		private readonly IGetIcaRecipies _getIcaRecipies = getIcaRecipies;
		private readonly IFavouriteRecipeService _favouriteRecipeService = favouriteRecipeService;

		public string? SearchString { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            SearchString = _searchService.GetSearchString();
            if (SearchString == string.Empty)
            {
                Recipes = await _recipeService.GetAsync();
            }
            else
            {
                Recipes = await _recipeService.GetBasedOnSearchAsync();
            }
            return Page();
        }
        public IActionResult OnPostEmptySearch()
        {
            _searchService.ClearSearchString();
            return RedirectToPage();
		}
		public async Task<IActionResult> OnPostLoadRecipes()
		{
            await _getIcaRecipies.GetIcaAsync();
			return RedirectToPage();
			
		}
		public async Task<IActionResult> OnPostAddToFavourites(int recipeId)
		{
            if (await _favouriteRecipeService.ExistsAsync(recipeId))
				return RedirectToPage();

			await _favouriteRecipeService.AddAsync(recipeId);
			return RedirectToPage();

		}
	}
}
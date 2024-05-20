using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace MasterMealMind.Web.Pages
{
    public class IndexModel(ISearchService searchService, 
        IRecipeService recipeService, 
        IGetIcaRecipes getIcaRecipes,
        IFavouriteRecipeService favouriteRecipeService) : PageModel
    {
		private readonly ISearchService _searchService = searchService;
		private readonly IRecipeService _recipeService = recipeService;
		private readonly IGetIcaRecipes _getIcaRecipes = getIcaRecipes;
		private readonly IFavouriteRecipeService _favouriteRecipeService = favouriteRecipeService;
		private readonly Stopwatch _stopwatch = new Stopwatch();

		public string? SearchString { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; } = [];

        public async Task<IActionResult> OnGetAsync(string searchWord)
        {
            SearchString = _searchService.GetSearchString();

            if (SearchString != string.Empty || searchWord != null) 
            {
                Recipes = await _recipeService.GetBasedOnSearchAsync(searchWord);
				SearchString = _searchService.GetSearchString();
                _searchService.ClearSearchString();
			}
			else
            {
                Recipes = await _recipeService.GetTenAsync();
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
			await _getIcaRecipes.GetIcaAsync();
			return RedirectToPage();
		}
		public async Task<IActionResult> OnPostAddToFavourites(int recipeId)
		{
			if (!await _favouriteRecipeService.ExistsAsync(recipeId))
				await _favouriteRecipeService.AddAsync(recipeId);

			return RedirectToPage();
		}
	}
}
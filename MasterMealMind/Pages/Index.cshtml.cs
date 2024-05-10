using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterMealMind.Web.Pages
{
    public class IndexModel(ISearchService searchService, IRecipeService recipeService, IGetIcaRecipies getIcaRecipies) : PageModel
    {
		private readonly ISearchService _searchService = searchService;
		private readonly IRecipeService _recipeService = recipeService;
		private readonly IGetIcaRecipies _getIcaRecipies = getIcaRecipies;

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
	}
}
using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterMealMind.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGetIcaRecipies _getIcaRecipies;
		private readonly IRecipeService recipeService;
		private readonly ISearchService _searchService;
		private readonly IRecipeService _recipeService;
		public string SearchString { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();


		public IndexModel(ISearchService searchService, IGetIcaRecipies getIcaRecipies, IRecipeService recipeService)
        {
            _searchService = searchService;
            _getIcaRecipies = getIcaRecipies;
			_recipeService = recipeService;
		}

        public async Task<IActionResult> OnGetAsync()
        {
            SearchString = _searchService.GetSearchString();
            var r = await _getIcaRecipies.GetAsync();
            var currentRecipeTitles = await _recipeService.GetTitlesAsync();
            var newRecipes = r.Where(x => !currentRecipeTitles.Contains(x.Title)).ToList();
            await _recipeService.AddAsync(newRecipes);
            Recipes = await _recipeService.GetAsync();
            return Page();
        }
        public IActionResult OnPostEmptySearch()
        {
            _searchService.ClearSearchString();
            return RedirectToPage();
        }
    }
}
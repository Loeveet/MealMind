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
        private readonly ISearchService _searchService; 
        public string SearchString { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();


		public IndexModel(ISearchService searchService, IGetIcaRecipies getIcaRecipies)
        {
            _searchService = searchService;
            _getIcaRecipies = getIcaRecipies;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            SearchString = _searchService.GetSearchString();
            Recipes = await _getIcaRecipies.Get();
            return Page();
        }
        public IActionResult OnPostEmptySearch()
        {
            _searchService.ClearSearchString();
            return RedirectToPage();
        }
    }
}
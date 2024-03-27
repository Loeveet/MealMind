using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Web.ApiServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MasterMealMind.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IIcaAPIService _icaAPIService;
        private readonly ISearchService _searchService;

        public string SearchString { get; set; }

        public RecipeResult RecipeResult { get; set; }


		public IndexModel(IIcaAPIService icaAPIService, ISearchService searchService)
        {
            _icaAPIService = icaAPIService;
            _searchService = searchService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            RecipeResult = await _icaAPIService.GetRecipes();
            SearchString = _searchService.GetSearchString();
            return Page();
        }
        public IActionResult OnPostEmptySearch()
        {
            _searchService.ClearSearchString();
            return RedirectToPage();
        }
    }
}
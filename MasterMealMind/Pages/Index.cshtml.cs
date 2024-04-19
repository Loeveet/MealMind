using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MasterMealMind.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGetIcaRecipies _getIcaRecipies;
        private readonly ISearchService _searchService; 
        public string SearchString { get; set; }

        public RecipeResult RecipeResult { get; set; }


		public IndexModel(ISearchService searchService, IGetIcaRecipies getIcaRecipies)
        {
            _searchService = searchService;
            _getIcaRecipies = getIcaRecipies;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //RecipeResult = await _icaAPIService.GetRecipes();
            SearchString = _searchService.GetSearchString();
            await _getIcaRecipies.Get();
            return Page();
        }
        public IActionResult OnPostEmptySearch()
        {
            _searchService.ClearSearchString();
            return RedirectToPage();
        }
    }
}
using MasterMealMind.Core.Models;
using MasterMealMind.Web.ApiServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterMealMind.Web.Pages
{
    public class RecipeDetailsPageModel : PageModel
    {
        private readonly IIcaAPIService _icaAPIService;

        public Recipe Recipe { get; set; }
        public RecipeDetailsPageModel(IIcaAPIService icaAPIService)
        {
            _icaAPIService = icaAPIService;
        }
        public async Task<IActionResult> OnGetAsync(int recipeId)
        {
            Recipe = await _icaAPIService.GetOneRecipe(recipeId);
            return Page();
        }
    }
}

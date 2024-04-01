using MasterMealMind.Core.Models;
using MasterMealMind.Web.ApiServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterMealMind.Web.Pages
{
    public class RecipeDetailsPageModel : PageModel
    {
        

        public Recipe Recipe { get; set; }
        public RecipeDetailsPageModel()
        {
            
        }
        public async Task<IActionResult> OnGetAsync(int recipeId)
        {
            // Recipe = await _icaAPIService.GetOneRecipe(recipeId);
            return Page();
        }
    }
}

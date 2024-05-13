using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterMealMind.Web.Pages
{
    public class RecipeDetailsPageModel(IFavouriteRecipeService favouriteRecipeService, IRecipeService recipeService) : PageModel
    {
        private readonly IFavouriteRecipeService _favouriteRecipeService = favouriteRecipeService;
        private readonly IRecipeService _recipeService = recipeService;

        public Recipe? Recipe { get; set; }
        public FavouriteRecipe? FavouriteRecipe { get; set; }
        public async Task<IActionResult> OnGetAsync(int recipeId, string source)
        {

			if (source == "recipes")
            {
                Recipe = await _recipeService.GetOneAsync(recipeId);
            }
            else if (source == "favourites")
            {
                FavouriteRecipe = await _favouriteRecipeService.GetOneAsync(recipeId);
            }
            return Page();
        }
    }
}

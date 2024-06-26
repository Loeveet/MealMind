using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterMealMind.Web.Pages
{

	public class FavouriteRecipesModel(IFavouriteRecipeService favouriteRecipeService) : PageModel
	{
		private readonly IFavouriteRecipeService _favouriteRecipeService = favouriteRecipeService;

		public IEnumerable<FavouriteRecipe> FavouriteRecipes { get; set; } = [];

		public async Task<IActionResult> OnGetAsync()
		{
			FavouriteRecipes = await _favouriteRecipeService.GetAsync();

			return Page();
		}
		public async Task<IActionResult> OnPostRemoveFromFavourites(int recipeId)
		{
			await _favouriteRecipeService.RemoveAsync(recipeId);
			return RedirectToPage();

		}
	}
}

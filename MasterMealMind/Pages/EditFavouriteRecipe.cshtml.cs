using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenQA.Selenium;
using MasterMealMind.Web.Helpers;

namespace MasterMealMind.Web.Pages
{
    public class EditFavouriteRecipeModel(IFavouriteRecipeService favouriteRecipeService) : PageModel
    {
		private readonly IFavouriteRecipeService _favouriteRecipeService = favouriteRecipeService;

		[BindProperty]
		public FavouriteRecipe? FavouriteRecipe { get; set; }

		public async Task<IActionResult> OnGetAsync(int recipeId)
		{

			FavouriteRecipe = await _favouriteRecipeService.GetOneAsync(recipeId) ?? throw new NotFoundException();
			
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string[] ingredients, string[] description, string title, string preamble)
		{
			if (!ModelState.IsValid || FavouriteRecipe == null)
			{
				return FavouriteRecipe == null ? RedirectToPage("/FavouriteRecipes") : RedirectToPage(new { recipeId = FavouriteRecipe.Id });
			}

			await _favouriteRecipeService.UpdateAsync(FavouriteRecipe.Id, ingredients, description, title, preamble);

			return RedirectToPage("/RecipeDetailsPage", new { recipeId = FavouriteRecipe.Id, source = "favourites" });
		}
	}
}

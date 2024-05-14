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
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var existingRecipe = await _favouriteRecipeService.GetOneAsync(FavouriteRecipe.Id) ?? throw new NotFoundException();


			existingRecipe.Title = title;
			existingRecipe.Preamble = preamble;
			existingRecipe.Ingredients = JoinString.JoinData(ingredients, "|");
			existingRecipe.Description = JoinString.JoinData(description, "|");

			await _favouriteRecipeService.UpdateAsync(existingRecipe);

			return RedirectToPage("/RecipeDetailsPage", new { recipeId = existingRecipe.Id, source = "favourites" });
		}
	}
}

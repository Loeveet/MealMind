using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenQA.Selenium;

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

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var existingRecipe = await _favouriteRecipeService.GetOneAsync(FavouriteRecipe.Id);

			if (existingRecipe == null)
			{
				throw new NotFoundException();
			}
			var ingredients = Request.Form["FavouriteRecipe.Ingredients[]"];

			existingRecipe.Ingredients = string.Join('|', ingredients);

			await _favouriteRecipeService.UpdateAsync(existingRecipe);

			return RedirectToPage("/EditFavouriteRecipe", new { recipeId = existingRecipe.Id });
		}
	}
}

using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasterMealMind.Web.Pages
{

	public class FavouriteRecipesModel(ISearchService searchService,
		IRecipeService recipeService,
		IGetIcaRecipes getIcaRecipies,
		IFavouriteRecipeService favouriteRecipeService) : PageModel
	{
		private readonly ISearchService _searchService = searchService;
		private readonly IRecipeService _recipeService = recipeService;
		private readonly IGetIcaRecipes _getIcaRecipies = getIcaRecipies;
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

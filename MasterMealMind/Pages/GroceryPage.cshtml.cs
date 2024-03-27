using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MasterMealMind.Web.ApiServices;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using MasterMealMind.Core.Interfaces;

namespace MasterMealMind.Web.Pages
{
    public class GroceryPageModel : PageModel
    {
        private readonly ILocalAPIService _localAPIService;
        private readonly ISearchService _searchService;

        public List<Grocery> Groceries { get; set; }

        [BindProperty]
        public Grocery NewGrocery { get; set; }
		public Grocery EditGrocery { get; set; }


		public GroceryPageModel(ILocalAPIService localAPIService, ISearchService searchService)
        {
            _localAPIService = localAPIService;
            _searchService = searchService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Groceries = await _localAPIService.HttpGetGroceriesAsync() ?? new List<Grocery>();

			if (TempData.ContainsKey("EditedGrocery"))
				NewGrocery = JsonConvert.DeserializeObject<Grocery>((string)TempData["EditedGrocery"]);
            
			return Page();
        }
        public async Task<IActionResult> OnPostAddOrUpdateGrocery()
        {
            if (NewGrocery != null && NewGrocery.Name != null)
                await _localAPIService.HttpPostGroceryAsync(NewGrocery);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteGrocery([FromForm] int deleteId)
        {
            if (await _localAPIService.HttpGetOneGroceryAsync(deleteId.ToString()) is null)
                return RedirectToPage();

            await _localAPIService.HttpDeleteGroceryAsync(deleteId.ToString());
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditGrocery([FromForm] int editId)
        {
            var editedGrocery = await _localAPIService.HttpGetOneGroceryAsync(editId.ToString());
            if (editedGrocery is null)
                return RedirectToPage();

            EditGrocery = editedGrocery;
            TempData["EditedGrocery"] = JsonConvert.SerializeObject(EditGrocery);
			

			return RedirectToPage();
		}

		public IActionResult OnPostAddToIngredientSearchList([FromForm] string selectedGroceryNames)
		{
            if (selectedGroceryNames is null)
                return RedirectToPage();

            _searchService.SetSearchString(selectedGroceryNames);
            return RedirectToPage("/Index");
		}
	}
}

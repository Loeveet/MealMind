using MasterMealMind.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Services;

namespace MasterMealMind.Web.Pages
{
    public class GroceryPageModel(IGroceryService groceryService, ISearchService searchService) : PageModel
    {
        private readonly IGroceryService _groceryService = groceryService;
        private readonly ISearchService _searchService = searchService;

        public IEnumerable<Grocery> Groceries { get; set; }

        [BindProperty]
        public Grocery NewGrocery { get; set; }
		public Grocery EditGrocery { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Groceries = await _groceryService.GetAllAsync() ?? [];

			if (TempData.ContainsKey("EditedGrocery"))
				NewGrocery = JsonConvert.DeserializeObject<Grocery>((string)TempData["EditedGrocery"]);
            
			return Page();
        }
        public async Task<IActionResult> OnPostAddOrUpdateGrocery()
        {
            if (NewGrocery != null && NewGrocery.Name != null)
                await _groceryService.AddOrUpdateAsync(NewGrocery);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteGrocery([FromForm] int deleteId)
        {
			if (!await _groceryService.GroceryExistsAsync(deleteId))
                return RedirectToPage();

            await _groceryService.DeleteAsync(deleteId);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditGrocery([FromForm] int editId)
        {
            var editedGrocery = await _groceryService.GetOneByIdAsync(editId);
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

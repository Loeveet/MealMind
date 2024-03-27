using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Core.Models;
using Microsoft.AspNetCore.Mvc;
using MasterMealMind.Core.Services;

namespace MasterMealMind.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroceryController : ControllerBase
    {
        private readonly IGroceryService _groceryService;

        public GroceryController(IGroceryService groceryService)
        {
            _groceryService = groceryService;
        }


        [HttpGet]
        public async Task<IEnumerable<Grocery>> GetGroceriesAsync() => await _groceryService.GetAllGroceriesAsync();

        [HttpGet("{id}")]
        public async Task<Grocery> GetOneGroceryByIdAsync(int id) => await _groceryService.GetOneGroceryAsync(id);

        [HttpPost]
        public async Task AddOrUpdateGroceryAsync([FromBody] Grocery grocery) => await _groceryService.AddOrUpdateGroceryAsync(grocery);


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroceryAsync(int id, [FromBody] Grocery grocery)
        {

            if (id != grocery.Id)
            {
                return BadRequest();
            }
            if (!await _groceryService.GroceryExistsAsync(grocery.Name))
            {
                return NotFound();
            }
            await _groceryService.UpdateGroceryAsync(grocery);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroceryAsync(int id)
        {
            if (!await _groceryService.GroceryExistsAsync(id))
            {
                return NotFound();
            }

            await _groceryService.DeleteGroceryAsync(id);

            return Ok();
        }
    }
}

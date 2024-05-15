using MasterMealMind.Core.Models;

namespace MasterMealMind.Core.Services
{
    public interface IGroceryService
    {
		Task<IEnumerable<Grocery>> GetAllAsync();

        Task<Grocery> GetOneByIdAsync(int id);
		Task<Grocery> GetOneByNameAsync(string name);

		Task AddOrUpdateAsync(Grocery modifiedGrocery);

        Task DeleteAsync(int id);

        Task<bool> GroceryExistsAsync(int id);
        Task<bool> GroceryExistsAsync(string name);

        Grocery GetGroceryToUpdate(Grocery updatedGrocery, Grocery originalGrocery);
    }
}

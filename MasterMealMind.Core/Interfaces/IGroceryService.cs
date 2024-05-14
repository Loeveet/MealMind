using MasterMealMind.Core.Models;

namespace MasterMealMind.Core.Services
{
    public interface IGroceryService
    {
		Task<IEnumerable<Grocery>> GetAllAsync();

        Task<Grocery> GetOneAsync(int id);

        Task AddOrUpdateAsync(Grocery modifiedGrocery);


        Task UpdateAsync(Grocery grocery);

        Task DeleteAsync(int id);

        Task<bool> GroceryExistsAsync(int id);
        Task<bool> GroceryExistsAsync(string name);

        Grocery GetGroceryToUpdate(Grocery updatedGrocery, Grocery originalGrocery);
    }
}

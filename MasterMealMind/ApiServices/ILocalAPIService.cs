using MasterMealMind.Core.Models;

namespace MasterMealMind.Web.ApiServices
{
    public interface ILocalAPIService
    {
        Task<List<Grocery>> HttpGetGroceriesAsync();
        Task<Grocery> HttpGetOneGroceryAsync(string requestUri);
		Task<bool> HttpPostGroceryAsync(Grocery grocery);
        Task<bool> HttpDeleteGroceryAsync(string requestUri);
        Task<bool> HttpUpdateGroceryAsync(Grocery grocery);
    }
}

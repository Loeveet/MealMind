using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Interfaces.IRepositories
{
	public interface IGroceryRepository
	{
		Task<IEnumerable<Grocery>> GetAllAsync();

		Task<Grocery> GetOneByIdAsync(int id);
		Task<Grocery> GetOneByNameAsync(string name);

		Task AddAsync(Grocery grocery);
		Task UpdateAsync(Grocery grocery);

		Task DeleteAsync(Grocery grocery);

		Task<bool> GroceryExistsAsync(int id);
		Task<bool> GroceryExistsAsync(string name);
	}
}

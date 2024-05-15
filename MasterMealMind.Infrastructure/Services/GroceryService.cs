
using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Core.Models;
using MasterMealMind.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using OpenQA.Selenium;
using MasterMealMind.Core.Interfaces.IRepositories;

namespace MasterMealMind.Infrastructure.Services
{
	public class GroceryService(IGroceryRepository groceryRepository) : IGroceryService
	{
		private readonly IGroceryRepository _groceryRepository = groceryRepository;

		public async Task<IEnumerable<Grocery>> GetAllAsync() => await _groceryRepository.GetAllAsync();

		public async Task<Grocery> GetOneByIdAsync(int id) => await _groceryRepository.GetOneByIdAsync(id);
		public async Task<Grocery> GetOneByNameAsync(string name) => await _groceryRepository.GetOneByNameAsync(name);


		public async Task AddOrUpdateAsync(Grocery modifiedGrocery)
		{
			if (await GroceryExistsAsync(modifiedGrocery.Name))
			{
				var existingGrocery = await GetOneByNameAsync(modifiedGrocery.Name);
				var updatedExistingGrocery = GetGroceryToUpdate(modifiedGrocery, existingGrocery);
				await _groceryRepository.UpdateAsync(updatedExistingGrocery);
			}
			else
				await _groceryRepository.AddAsync(modifiedGrocery);
		}

		public async Task DeleteAsync(int id)
		{
			var grocery = await GetOneByIdAsync(id);
			await _groceryRepository.DeleteAsync(grocery);
		}

		public async Task<bool> GroceryExistsAsync(int id) => await _groceryRepository.GroceryExistsAsync(id);

		public async Task<bool> GroceryExistsAsync(string name) => await _groceryRepository.GroceryExistsAsync(name);

		public Grocery GetGroceryToUpdate(Grocery updatedGrocery, Grocery originalGrocery)
		{
			originalGrocery.Name = updatedGrocery.Name;
			originalGrocery.Quantity = updatedGrocery.Quantity;
			originalGrocery.Description = updatedGrocery.Description;
			originalGrocery.Unit = updatedGrocery.Unit;
			originalGrocery.Storage = updatedGrocery.Storage;

			return originalGrocery;

		}
	}
}

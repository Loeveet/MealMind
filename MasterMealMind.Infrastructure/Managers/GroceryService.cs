
using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Core.Models;
using MasterMealMind.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MasterMealMind.Infrastructure.Managers
{
    public class GroceryService : IGroceryService
    {
        private readonly MyDbContext _context;

        public GroceryService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Grocery>> GetAllAsync()
        {
            return await _context.Groceries.ToListAsync();
        }

        public async Task<Grocery> GetOneAsync(int id)
        {
            var grocery = await _context.Groceries.SingleOrDefaultAsync(g => g.Id == id);

            if (grocery == null)
                throw new InvalidOperationException($"Grocery with ID {id} not found.");

            return grocery;
        }

        public async Task AddOrUpdateAsync(Grocery modifiedGrocery)
        {
            if (await GroceryExistsAsync(modifiedGrocery.Name))
            {
                var existingGrocery = await _context.Groceries.FirstOrDefaultAsync(g => g.Name == modifiedGrocery.Name);
                var updatedExistingGrocery = GetGroceryToUpdate(modifiedGrocery, existingGrocery);
                _context.Entry(updatedExistingGrocery).State = EntityState.Modified;

            }

            else
                await _context.Groceries.AddAsync(modifiedGrocery);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Grocery grocery)
        {

            var groceryToUpdate = await _context.Groceries.FirstOrDefaultAsync(g => string.Equals(g.Name, grocery.Name, StringComparison.OrdinalIgnoreCase)) ?? throw new ArgumentNullException("updateGrocery");
            var updatedGrocery = GetGroceryToUpdate(grocery, groceryToUpdate);
            _context.Entry(updatedGrocery).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var grocery = await _context.Groceries.FindAsync(id);

            if (grocery == null)
            {
                throw new InvalidOperationException();
            }
            _context.Groceries.Remove(grocery);
            _context.SaveChanges();

        }

        public async Task<bool> GroceryExistsAsync(int id)
        {
            return await _context.Groceries.AnyAsync(g => g.Id == id);
        }
        public async Task<bool> GroceryExistsAsync(string name)
        {
            return await _context.Groceries.AnyAsync(g => g.Name == name);
        }

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

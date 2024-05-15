using MasterMealMind.Core.Interfaces.IRepositories;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Repositories
{
	public class GroceryRepository(MyDbContext context) : IGroceryRepository
	{
		private readonly MyDbContext _context = context;


		public async Task<IEnumerable<Grocery>> GetAllAsync() => await _context.Groceries.ToListAsync() ?? [];


		public async Task DeleteAsync(Grocery grocery)
		{
			_context.Groceries.Remove(grocery);
			await _context.SaveChangesAsync();
		}

		public async Task<Grocery> GetOneByIdAsync(int id) => await _context.Groceries.FindAsync(id) ?? throw new NotFoundException();
		public async Task<Grocery> GetOneByNameAsync(string name) => await _context.Groceries.FirstOrDefaultAsync(x => x.Name == name) ?? throw new NotFoundException();

		public async Task<bool> GroceryExistsAsync(int id) => await _context.Groceries.AnyAsync(g => g.Id == id);
		public async Task<bool> GroceryExistsAsync(string name) => await _context.Groceries.AnyAsync(g => g.Name == name);
		public async Task UpdateAsync(Grocery grocery)
		{
			_context.Entry(grocery).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
		public async Task AddAsync(Grocery grocery)
		{
			await _context.Groceries.AddAsync(grocery);
			await _context.SaveChangesAsync();
		}
	}
}

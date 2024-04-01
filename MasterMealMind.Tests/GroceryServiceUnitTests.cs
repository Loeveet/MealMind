using MasterMealMind.Core.Models;
using System.Net;
using System.Text.Json;
using Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MasterMealMind.Web.Pages;
using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MasterMealMind.Tests
{
	public class GroceryServiceUnitTests
	{
        
		[Fact]
		public void GroceryToUpdate_ShouldUpdateExistingGrocery()
		{
			// Arrange

			var originalGrocery = new Grocery { Id = 1, Name = "Tomato", Quantity = 2, Description = "Red" };
			var updatedGrocery = new Grocery { Id = 1, Name = "Tomato", Quantity = 3, Description = "Red" };

			var options = new DbContextOptionsBuilder<MyDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryDatabase5")
				.Options;

			using (var dbContext = new MyDbContext(options))
			{
				var sut = new GroceryService(dbContext);

				// Act
				var result = sut.GetGroceryToUpdate(updatedGrocery, originalGrocery);

				// Assert
				Assert.NotNull(result);
				Assert.Equal(updatedGrocery.Name, result.Name, StringComparer.OrdinalIgnoreCase);
				Assert.Equal(updatedGrocery.Quantity, result.Quantity);
				Assert.Equal(updatedGrocery.Description, result.Description);
				Assert.Equal(updatedGrocery.Unit, result.Unit);
				Assert.Equal(updatedGrocery.Storage, result.Storage);
			}
		}

		[Fact]
		public async Task AddOrUpdateGrocery_ExistingGrocery_ShouldUpdateToDb()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<MyDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryDatabase4")
				.Options;

			using (var dbContext = new MyDbContext(options))
			{
				var existingGrocery = new Grocery { Name = "ExistingGrocery" };
				dbContext.Groceries.Add(existingGrocery);
				dbContext.SaveChanges();

				var sut = new GroceryService(dbContext);

				// Act
				var modifiedGrocery = new Grocery { Name = "ExistingGrocery", Quantity = 5 };
				await sut.AddOrUpdateAsync(modifiedGrocery);
				var updatedGrocery = await dbContext.Groceries.FirstOrDefaultAsync(g => g.Name == "ExistingGrocery");

				// Assert
				Assert.NotNull(updatedGrocery);
				Assert.Equal(5, updatedGrocery.Quantity);
			}
		}

		[Fact]
		public async Task AddOrUpdateGrocery_NewGrocery_ShouldAddToDb()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<MyDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryDatabase1")
				.Options;

			using (var dbContext = new MyDbContext(options))
			{
				var sut = new GroceryService(dbContext);

				// Act
				var newGrocery = new Grocery { Name = "NewGrocery", Quantity = 10 };
				await sut.AddOrUpdateAsync(newGrocery);
				var addedGrocery = await dbContext.Groceries.FirstOrDefaultAsync(g => g.Name == "NewGrocery");

				// Assert
				Assert.NotNull(addedGrocery);
				Assert.Equal(10, addedGrocery.Quantity);
			}
		}

		[Fact]
		public async Task DeleteGrocery_ExistingGrocery_ShouldRemoveFromDatabase()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<MyDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryDatabase2")
				.Options;

			using (var dbContext = new MyDbContext(options))
			{
				var existingGrocery = new Grocery { Id = 1, Name = "ExistingGrocery" };
				dbContext.Groceries.Add(existingGrocery);
				dbContext.SaveChanges();

				var sut = new GroceryService(dbContext);

				// Act
				await sut.DeleteAsync(1);
				var deletedGrocery = await dbContext.Groceries.FindAsync(1);

				// Assert
				Assert.Null(deletedGrocery);
			}
		}

		[Fact]
		public async Task DeleteGrocery_NonExistingGrocery_ShouldThrowException()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<MyDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryDatabase3")
				.Options;

			using (var dbContext = new MyDbContext(options))
			{
				var sut = new GroceryService(dbContext);

				// Act and Assert
				await Assert.ThrowsAsync<InvalidOperationException>(() => sut.DeleteAsync(1));
			}
		}
	}
}
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
using MasterMealMind.Core.Interfaces.IRepositories;
using MasterMealMind.Infrastructure.Repositories;

namespace MasterMealMind.Tests.Tests
{
	public class GroceryServiceUnitTests
	{

		[Fact]
		public void GroceryToUpdate_ShouldUpdateExistingGrocery()
		{
			// Arrange
			var originalGrocery = new Grocery { Id = 1, Name = "Tomato", Quantity = 2, Description = "Red" };
			var updatedGrocery = new Grocery { Id = 1, Name = "Tomato", Quantity = 3, Description = "Red" };

			var sut = new GroceryService(null);

			// Act
			var result = sut.GetGroceryToUpdate(updatedGrocery, originalGrocery);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(updatedGrocery.Name, result.Name, StringComparer.OrdinalIgnoreCase);
			Assert.Equal(updatedGrocery.Quantity, result.Quantity);
			Assert.Equal(updatedGrocery.Description, result.Description);
		}
		[Fact]
		public async void AddOrUpdateAsync_ShouldAdd_IfNotExists()
		{
			//Arrange
			var groceryRepositoryMock = new Mock<IGroceryRepository>();
			groceryRepositoryMock.Setup(repo => repo.GroceryExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
			groceryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Grocery>())).Verifiable();
			var sut = new GroceryService(groceryRepositoryMock.Object);

			var newGrocery = new Grocery { Id = 1, Name = "Tomato", Quantity = 2, Description = "Red" };

			//Act
			await sut.AddOrUpdateAsync(newGrocery);

			//Assert
			groceryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Grocery>()), Times.Once);
			groceryRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Grocery>()), Times.Never);

		}

		//[Fact]
		//public async void AddOrUpdateAsync_ShouldUpdate_IfExists()
		//{
		//	//Arrange
		//	var groceryRepositoryMock = new Mock<IGroceryRepository>();
		//	groceryRepositoryMock.Setup(repo => repo.GroceryExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
			
		//	var existingGrocery = new Grocery { Id = 1, Name = "Tomato", Quantity = 2, Description = "Red" };
		//	var updatedExistingGrocery = new Grocery { Id = 1, Name = "Tomato", Quantity = 3, Description = "Red" };

		//	var groceryServiceMock = new Mock<IGroceryService>();
		//	groceryRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Grocery>())).Verifiable();
		//	groceryRepositoryMock.Setup(repo => repo.GetOneByNameAsync(It.IsAny<string>())).Verifiable();

		//	groceryServiceMock.Setup(service => service.GetOneByNameAsync(existingGrocery.Name)).ReturnsAsync(existingGrocery);
		//	groceryServiceMock.Setup(service => service.GetGroceryToUpdate(It.IsAny<Grocery>(), existingGrocery)).Returns(updatedExistingGrocery);

		//	var sut = new GroceryService(groceryRepositoryMock.Object);

		//	//Act
		//	await sut.AddOrUpdateAsync(existingGrocery);

		//	//Assert
		//	groceryRepositoryMock.Verify(repo => repo.UpdateAsync(updatedExistingGrocery), Times.Once);
		//	groceryRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Grocery>()), Times.Never);
		//}
	}
}
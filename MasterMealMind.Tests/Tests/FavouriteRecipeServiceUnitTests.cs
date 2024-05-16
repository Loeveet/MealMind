using MasterMealMind.Core.Interfaces.IRepositories;
using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using MasterMealMind.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Tests.Tests
{
	public class FavouriteRecipeServiceUnitTests
	{
		[Fact]
		public async Task AddAsync_ShouldAddFavouriteRecipeCorrectly()
		{
			// Arrange
			var recipeId = 1;
			var recipe = new Recipe { Id = recipeId, Title = "Test Recipe" };
			var favouriteRecipeRepositoryMock = new Mock<IFavouriteRecipeRepository>();
			favouriteRecipeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<FavouriteRecipe>())).Verifiable();

			var recipeServiceMock = new Mock<IRecipeService>();
			recipeServiceMock.Setup(service => service.GetOneAsync(recipeId)).ReturnsAsync(recipe);

			var favouriteRecipeService = new FavouriteRecipeService(favouriteRecipeRepositoryMock.Object, recipeServiceMock.Object);

			// Act
			await favouriteRecipeService.AddAsync(recipeId);

			// Assert
			favouriteRecipeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<FavouriteRecipe>()), Times.Once);
		}

		[Theory]
		[InlineData(1, true)]
		[InlineData(2, false)]
		public async Task ExistsAsync_ShouldReturnCorrectValue(int recipeId, bool expectedResult)
		{
			// Arrange
			var favouriteRecipeRepositoryMock = new Mock<IFavouriteRecipeRepository>();
			favouriteRecipeRepositoryMock.Setup(repo => repo.ExistsAsync(recipeId)).ReturnsAsync(expectedResult);

			var favouriteRecipeService = new FavouriteRecipeService(favouriteRecipeRepositoryMock.Object, null);

			// Act
			var result = await favouriteRecipeService.ExistsAsync(recipeId);

			// Assert
			Assert.Equal(expectedResult, result);
		}
	}
}

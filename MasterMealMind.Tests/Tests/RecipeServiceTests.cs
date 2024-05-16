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
	public class RecipeServiceTests
	{
		[Fact]
		public async Task GetBasedOnSearchAsync_ShouldReturnRecipesBasedOnSearch()
		{
			// Arrange
			var input = "chicken pasta";
			var searchString = "chicken pasta";
			var searchWords = searchString.Split(' ');
			var expectedRecipes = new List<Recipe> { new Recipe { Id = 1, Title = "Chicken Pasta" } };

			var searchServiceMock = new Mock<ISearchService>();
			searchServiceMock.Setup(service => service.GetSearchString()).Returns(searchString);

			var recipeRepositoryMock = new Mock<IRecipeRepository>();
			recipeRepositoryMock.Setup(repo => repo.GetBasedOnSearchAsync(searchWords)).ReturnsAsync(expectedRecipes);

			var sut = new RecipeService(recipeRepositoryMock.Object, searchServiceMock.Object);

			// Act
			var result = await sut.GetBasedOnSearchAsync(input);

			// Assert
			Assert.Equal(expectedRecipes, result);
		}

		[Fact]
		public async Task GetBasedOnSearchAsync_ShouldSetSearchString_IfEmpty()
		{
			// Arrange
			var input = "chicken pasta";
			var searchString = string.Empty;
			var searchWords = input.Split(' ');
			var expectedRecipes = new List<Recipe> { new Recipe { Id = 1, Title = "Chicken Pasta" } };

			var searchServiceMock = new Mock<ISearchService>();
			searchServiceMock.Setup(service => service.GetSearchString()).Returns(searchString);
			searchServiceMock.Setup(service => service.SetSearchString(input)).Verifiable();

			var recipeRepositoryMock = new Mock<IRecipeRepository>();
			recipeRepositoryMock.Setup(repo => repo.GetBasedOnSearchAsync(searchWords)).ReturnsAsync(expectedRecipes);

			var sut = new RecipeService(recipeRepositoryMock.Object, searchServiceMock.Object);

			// Act
			var result = await sut.GetBasedOnSearchAsync(input);

			// Assert
			searchServiceMock.Verify(service => service.SetSearchString(input), Times.Once);
			Assert.Equal(expectedRecipes, result);
		}

	}
}

using MasterMealMind.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Tests.Tests
{

    public class SearchServiceTests
    {
        [Fact]
        public void GetSearchString_ShouldReturnInitialString()
        {
            // Arrange
            var sut = new SearchService();

            // Act
            string result = sut.GetSearchString();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void SetSearchString_ShouldSetSearchString()
        {
            // Arrange
            var sut = new SearchService();
            string searchString = "test";

            // Act
            sut.SetSearchString(searchString);

            // Assert
            Assert.Equal(searchString, sut.GetSearchString());
        }

        [Fact]
        public void ClearSearchString_ShouldSetEmptyString()
        {
            // Arrange
            var sut = new SearchService();
            sut.SetSearchString("test");

            // Act
            sut.ClearSearchString();

            // Assert
            Assert.Equal(string.Empty, sut.GetSearchString());
        }
    }

}


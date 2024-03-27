using MasterMealMind.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Tests
{
    public class ProfanityFilterServiceTests
    {
        [Theory]
        [InlineData("This is a test", "This is a test", 1000)]
        [InlineData("jävla test", "**** test", 1000)]
        [InlineData("idiot and jävel", "**** and ****", 1000)]
        [InlineData("NoProfanityHere", "NoProfanityHere", 1000)]
        [InlineData("", "", 1000)]
        [InlineData(null, null, 1000)]
        public void FilterProfanity_ShouldFilterCorrectly(string input, string expectedOutput, int timeoutMilliseconds)
        {
            // Arrange
            var sut = new ProfanityFilterService();

            // Act
            string result = sut.FilterProfanity(input, timeoutMilliseconds);

            // Assert
            Assert.Equal(expectedOutput, result);
        }

        [Fact]
        public void FilterProfanity_ShouldNotTimeout()
        {
            // Arrange
            var sut = new ProfanityFilterService();
            var input = "This is a test string without profanity";
            var timeoutMilliseconds = 1000;

            // Act
            var result = sut.FilterProfanity(input, timeoutMilliseconds);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FilterProfanity_ShouldTimeout()
        {
            // Arrange
            var sut = new ProfanityFilterService();
            var shortInput = "This is a short test string.";
            var repetitionCount = 1000000;
            var input = string.Concat(Enumerable.Repeat(shortInput, repetitionCount));
            var timeoutMilliseconds = 1;

            // Act and Assert
            Assert.Throws<TaskCanceledException>(() => sut.FilterProfanity(input, timeoutMilliseconds));
        }
    }


}

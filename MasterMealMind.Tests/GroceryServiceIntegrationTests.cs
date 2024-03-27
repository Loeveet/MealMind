using MasterMealMind.Core.Models;
using MasterMealMind.Core.Services;
using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Web.ApiServices;
using MasterMealMind.Web.Pages;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace MasterMealMind.Tests
{
    public class GroceryServiceIntegrationTests : IClassFixture<WebApplicationFactory<API.Program>>
    {
        private readonly HttpClient _httpClient;
        public GroceryServiceIntegrationTests(WebApplicationFactory<API.Program> factory)
        {
            _httpClient = factory.CreateDefaultClient();
        }


        [Fact]
        public async Task CanGetListOfAllGroceries()
        {
            var response = await _httpClient.GetAsync("api/groceries");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }
    }
}

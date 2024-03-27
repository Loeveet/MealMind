using MasterMealMind.Core.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration.UserSecrets;
using MasterMealMind.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MasterMealMind.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MasterMealMind.Web.ApiServices
{
    public class IcaAPIService : IIcaAPIService
    {
        private static readonly string _baseUrl = "https://handla.api.ica.se/";
        private readonly IConfiguration _configuration;
        private readonly ISearchService _searchService;
        public IcaAPIService(ISearchService searchService)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<IcaAPIService>();
            _configuration = configurationBuilder.Build();
            _searchService = searchService;
        }


        public async Task<string> GetAuthenticationTicket(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/api/login/");
                response.EnsureSuccessStatusCode();

                string authenticationTicket = response.Headers.GetValues("AuthenticationTicket").FirstOrDefault();

                return authenticationTicket;
            }
        }

        public async Task<RecipeResult> GetRecipes()
        {
            string authenticationTicket = await GetAuthenticationTicket(_configuration["YourConfigKey:Username"], _configuration["YourConfigKey:Password"]);

            using (HttpClient client = new HttpClient())
            {
                RecipeResult result = null;
                var phrase = _searchService.GetSearchString();
                string searchUri = $"searchwithfilters?recordsPerPage=40&pageNumber=1&phrase={phrase}&sorting=0";
                if (phrase.IsNullOrEmpty())
                    searchUri = "random?numberofrecipes=10";



                // Add AuthenticationTicket to the request headers
                client.DefaultRequestHeaders.Add("AuthenticationTicket", authenticationTicket);

                // Make a GET request to /api/recipes/searchwithfilters
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/api/recipes/{searchUri}");
                response.EnsureSuccessStatusCode();

                // Read and deserialize the response content (JSON)
                string responseBody = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<RecipeResult>(responseBody);
                return result;
            }
        }

        public async Task<Recipe> GetOneRecipe(int recipeId)
        {
            string authenticationTicket = await GetAuthenticationTicket(_configuration["YourConfigKey:Username"], _configuration["YourConfigKey:Password"]);

            using (HttpClient client = new HttpClient())
            {
                Recipe result = null;

                // Add AuthenticationTicket to the request headers
                client.DefaultRequestHeaders.Add("AuthenticationTicket", authenticationTicket);

                // Make a GET request to /api/recipes/searchwithfilters
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/api/recipes/recipe/{recipeId}");
                response.EnsureSuccessStatusCode();

                // Read and deserialize the response content (JSON)
                string responseBody = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<Recipe>(responseBody);
                return result;
            }
        }


    }
}

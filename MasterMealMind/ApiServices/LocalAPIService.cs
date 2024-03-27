using MasterMealMind.Core.Models;
using System.Text.Json;
using MasterMealMind.API.Controllers;

namespace MasterMealMind.Web.ApiServices
{
	public class LocalAPIService : ILocalAPIService
	{
		private readonly HttpClient _httpClient;
		private const string Base_Address = "https://localhost:44338/api/";
		private const string Uri = "Grocery/";

		public LocalAPIService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(Base_Address);
		}

		public async Task<List<Grocery>> HttpGetGroceriesAsync()
		{
			var response = await _httpClient.GetAsync(Uri);
			var content = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<Grocery>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		}
		public async Task<Grocery> HttpGetOneGroceryAsync(string requestUri)
		{
			var response = await _httpClient.GetAsync(Uri + requestUri);
			var content = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<Grocery>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		}
		public async Task<bool> HttpPostGroceryAsync(Grocery grocery)
		{
			var jsonString = JsonSerializer.Serialize(grocery);
			var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("Grocery", content);
			return response.IsSuccessStatusCode;
		}
		public async Task<bool> HttpUpdateGroceryAsync(Grocery grocery)
		{
			var jsonString = JsonSerializer.Serialize(grocery);
			var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync("Grocery/" + grocery.Id, content);
			if (!response.IsSuccessStatusCode)
				return false;
			return true;
		}
		public async Task<bool> HttpDeleteGroceryAsync(string requestUri)
		{
			var response = await _httpClient.DeleteAsync(Uri + requestUri);
			if (!response.IsSuccessStatusCode)
				return false;
			return true;
		}

	}


}

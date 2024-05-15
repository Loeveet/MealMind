using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Services;
using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.DevTools.V122.Page;

namespace MasterMealMind.Web
{
	public class Program
	{

		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			var connectionString = builder.Configuration.GetConnectionString("DEFAULTCONNECTION")
				?? Environment.GetEnvironmentVariable("DEFAULTCONNECTION")
				?? throw new InvalidOperationException("Connection string 'DEFAULTCONNECTION' not found.");

			builder.Services.AddDbContext<MyDbContext>(options =>
				options.UseSqlServer(connectionString));



			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddScoped<IGroceryService, GroceryService>();
			builder.Services.AddScoped<ISearchService, SearchService>();
			builder.Services.AddScoped<IRecipeService, RecipeService>();
			builder.Services.AddScoped<IGetIcaRecipies, GetIcaRecipies>();
			builder.Services.AddScoped<IFavouriteRecipeService, FavouriteRecipeService>();


			var app = builder.Build();
			if (args.Length > 0 && args[0] == "RunScraper")
			{
				using var scope = app.Services.CreateScope();
				var getIcaRecipies = app.Services.GetRequiredService<IGetIcaRecipies>();
				await RunScraper(getIcaRecipies);
			}
			else
			{
				// Configure the HTTP request pipeline.
				if (!app.Environment.IsDevelopment())
				{
					app.UseExceptionHandler("/Error");
					// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
					app.UseHsts();
				}

				app.UseHttpsRedirection();
				app.UseStaticFiles();

				app.UseRouting();

				app.UseAuthorization();

				app.MapRazorPages();

				app.Run();
			}
		}
		public static async Task RunScraper(IGetIcaRecipies getIcaRecipies)
		{
			await getIcaRecipies.GetIcaAsync();
		}
	}
}
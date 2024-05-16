using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Interfaces.IRepositories;
using MasterMealMind.Core.Services;
using MasterMealMind.Infrastructure.Repositories;
using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Scraper.Scrapers;
using Microsoft.EntityFrameworkCore;

namespace MasterMealMind.Web
{
	public class Program
	{

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			var connectionString = builder.Configuration.GetConnectionString("DEFAULTCONNECTION")
				?? throw new InvalidOperationException("Connection string 'DEFAULTCONNECTION' not found.");

			builder.Services.AddDbContext<MyDbContext>(options =>
				options.UseSqlServer(connectionString));



			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddScoped<IGroceryService, GroceryService>();
			builder.Services.AddScoped<ISearchService, SearchService>();
			builder.Services.AddScoped<IRecipeService, RecipeService>();
			builder.Services.AddScoped<IGetIcaRecipes, GetIcaRecipes>();
			builder.Services.AddScoped<IFavouriteRecipeService, FavouriteRecipeService>();
			builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
			builder.Services.AddScoped<IGroceryRepository, GroceryRepository>();
			builder.Services.AddScoped<IFavouriteRecipeRepository, FavouriteRecipeRepository>();
			builder.Services.AddScoped<IICAScraper, ICAscraper>();


			var app = builder.Build();

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
}
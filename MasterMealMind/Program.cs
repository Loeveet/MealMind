using MasterMealMind.Core.Enum;
using MasterMealMind.Core.Interfaces;
using MasterMealMind.Infrastructure.Services;
using MasterMealMind.Web.ApiServices;
using Microsoft.AspNetCore.Mvc;

namespace MasterMealMind.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
			builder.Services.AddScoped<ILocalAPIService, LocalAPIService>();
			builder.Services.AddScoped<IIcaAPIService, IcaAPIService>();
			builder.Services.AddScoped<HttpClient>();
            builder.Services.AddSingleton<ISearchService, SearchService>();


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
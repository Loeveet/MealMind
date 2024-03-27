
using MasterMealMind.Core.Services;
using MasterMealMind.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

namespace MasterMealMind.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IGroceryService, GroceryService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();
            builder.Services.AddEndpointsApiExplorer();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapHealthChecks("api/healthcheck");

            app.MapGet("api/groceries", async (IGroceryService service) =>
            {
                var groceries = await service.GetAllGroceriesAsync();
                return groceries;
            })
            .WithName("GetGroceries");

            app.MapControllers();

            app.Run();
        }
    }
}
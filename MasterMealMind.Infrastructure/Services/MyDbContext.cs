using Microsoft.EntityFrameworkCore;
using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Services
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {            

        }

        public DbSet<Grocery> Groceries { get; set;}
		public DbSet<Recipe> Recipes { get; set; }


	}
}

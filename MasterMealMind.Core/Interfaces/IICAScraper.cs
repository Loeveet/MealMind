using MasterMealMind.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Interfaces
{
	public interface IICAScraper
	{
		Task<List<Recipe>> GetAsync(string url);
	}
}

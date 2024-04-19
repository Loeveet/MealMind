using System.Text.Json.Serialization;

namespace MasterMealMind.Core.Models
{

	public class Recipe
	{
		public int Id { get; set; }
        public string? Title { get; set; }
        public string? Desc { get; set; }
        public List<string> Ingredients { get; set; } = [];

    }
}

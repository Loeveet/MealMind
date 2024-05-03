using System.Text.Json.Serialization;

namespace MasterMealMind.Core.Models
{

	public class Recipe
	{
		public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
		public string? Preamble { get; set; }
		public string? ImgURL { get; set; }
		public string? Ingredients { get; set; }

    }
}

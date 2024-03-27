namespace MasterMealMind.Core.Models
{
	public class RecipeResult
	{
		public int NumberOfPages { get; set; }
		public List<Recipe> Recipes { get; set; }
		public int TotalNumberOfRecipes { get; set; }
		public string Msg { get; set; }
	}
}

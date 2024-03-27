using System.Text.Json.Serialization;

namespace MasterMealMind.Core.Models
{

	public class Recipe
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int ImageId { get; set; }
		public string ImageUrl { get; set; }
		public string YouTubeId { get; set; }
		public Ingredientgroup[] IngredientGroups { get; set; }
		public Extraportion[] ExtraPortions { get; set; }
		public string PreambleHTML { get; set; }
		public Grocerybags GroceryBags { get; set; }
		public string PreparationAdvice { get; set; }
		public string DietaryInfo { get; set; }
		public Nutritionperportion NutritionPerPortion { get; set; }
		public bool IsGoodClimateChoice { get; set; }
		public bool IsKeyHole { get; set; }
		public string[] CookingSteps { get; set; }
		public Cookingstepswithtimer[] CookingStepsWithTimers { get; set; }
		public string CurrentUsersRating { get; set; }
		public string AverageRating { get; set; }
		public string Difficulty { get; set; }
		public string CookingTime { get; set; }
		public string CookingTimeAbbreviated { get; set; }
		public int Portions { get; set; }
		public string PortionsDescription { get; set; }
		public object[] Categories { get; set; }
		public string[] MdsaCategories { get; set; }
		public int[] MoreLikeThis { get; set; }
		public int OfferCount { get; set; }
		public int CommentCount { get; set; }
	}

	public class Grocerybags
	{
		public Bag[] Bags { get; set; }
		public string Url { get; set; }
	}

	public class Bag
	{
		public int Year { get; set; }
		public int WeekNumber { get; set; }
		public int ArticleNumber { get; set; }
	}

	public class Nutritionperportion
	{
		public float Carbohydrate { get; set; }
		public float Fat { get; set; }
		public float Protein { get; set; }
		public float KCalories { get; set; }
		public float KJoule { get; set; }
	}

	public class Ingredientgroup
	{
		public int Portions { get; set; }
		public Ingredient[] Ingredients { get; set; }
		public string GroupName { get; set; }
	}

	public class Ingredient
	{
		public string Text { get; set; }
		public int IngredientId { get; set; }
		public float Quantity { get; set; }
		public float MinQuantity { get; set; }
		public string QuantityFraction { get; set; }
		[JsonPropertyName("Ingredient")]

		public string IngredientName { get; set; }
		public string Unit { get; set; }
	}

	public class Extraportion
	{
		public int Portions { get; set; }
		public Ingredient[] Ingredients { get; set; }
		public string GroupName { get; set; }
	}


	public class Cookingstepswithtimer
	{
		public string Description { get; set; }
		public int?[] TimersInMinutes { get; set; }
	}

}

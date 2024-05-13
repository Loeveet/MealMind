namespace MasterMealMind.Web.Helpers
{
    public static class IngredientFormatter
    {
        public static string FormatingIngredient(string ingredient)
        {
            var trimmedIngredient = ingredient.Trim();
            var isHeader = trimmedIngredient.StartsWith("*");

            if (isHeader)
            {
                return $"<strong>{trimmedIngredient[1..]}</strong>";
            }
            else if (!string.IsNullOrEmpty(trimmedIngredient))
            {
                var ingredientWithFirstLetterUpperCase = char.ToUpper(trimmedIngredient[0]) + trimmedIngredient.Substring(1);
                return ingredientWithFirstLetterUpperCase;
            }
            else
            {
                return trimmedIngredient;
            }
        }
    }
}

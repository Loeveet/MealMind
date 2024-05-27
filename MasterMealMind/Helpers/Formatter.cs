namespace MasterMealMind.Web.Helpers
{
    public static class Formatter
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
                var ingredientWithFirstLetterUpperCase = char.ToUpper(trimmedIngredient[0]) + trimmedIngredient[1..];
                return ingredientWithFirstLetterUpperCase;
            }
            else
            {
                return trimmedIngredient;
            }
        }
		public static string FormatDescription(string desc)
		{
			var trimmedDesc = desc.Trim();
			var colonIndex = trimmedDesc.IndexOf(':');

			if (colonIndex != -1)
			{
				var beforeColon = trimmedDesc[..colonIndex].Trim();
				var afterColon = trimmedDesc[(colonIndex + 1)..].Trim();
				return $"<strong>{beforeColon}</strong>: {afterColon}";
			}
			else
			{
				return trimmedDesc;
			}
		}
	}
}

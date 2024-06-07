using HtmlAgilityPack;
using MasterMealMind.Core.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MasterMealMind.Scraper.Helpers
{
	public class ScraperHelpers
	{

		public static string DecodeHtml(string html)
		{
			return WebUtility.HtmlDecode(html);
		}

		public static string UseRegex(string description)
		{

			var patterns = new Dictionary<string, string>
			{
				{ @"Öppna timer: \d+ min(uter)?(?: \d+ sek)?", string.Empty },
				{ @"\.(?!\s)", ". " },
				{ @"Näringsvärde.*", string.Empty }
			};

			foreach (var pattern in patterns)
			{
				description = Regex.Replace(description, pattern.Key, pattern.Value);
			}

			return description;

		}

		public static string FilterIngredients(HtmlNodeCollection ingredientGroups)
		{
			if (ingredientGroups == null)
				return string.Empty;

			var combinedList = new List<string>();

			foreach (var group in ingredientGroups)
			{
				var headerNode = group.SelectSingleNode(".//h3[contains(@class, 'ingredients-list-group__heading')]");
				var ingredientNodes = group.SelectNodes(".//div[contains(@class, 'ingredients-list-group__card')]");

				if (headerNode != null)
				{
					var headerText = DecodeHtml(headerNode.InnerText.Trim());
					combinedList.Add("*" + headerText);
				}

				if (ingredientNodes == null)
					continue;

				var ingredientsText = ingredientNodes
				.Select(ing =>
				{
					var quantityElement = ing.SelectSingleNode(".//span[contains(@class, 'ingredients-list-group__card__qty')]");
					var ingredientNameElement = ing.SelectSingleNode(".//span[contains(@class, 'ingredients-list-group__card__ingr')]");

					if (ingredientNameElement == null)
						return null;

					var quantity = quantityElement != null ? DecodeHtml(quantityElement.InnerText.Trim()) : "";
					var ingredientName = DecodeHtml(ingredientNameElement.InnerText.Trim());
					var ingredientText = string.IsNullOrWhiteSpace(quantity) ? ingredientName : $"{quantity} {ingredientName}";
					return UseRegex(ingredientText);
				}).ToList();

				combinedList.AddRange(ingredientsText);
			}
			return string.Join("| ", combinedList);
		}
		public static string FilterDescription(HtmlNodeCollection descriptionElement)
		{
			if (descriptionElement == null)
				return string.Empty;

			var steps = new List<string>();
			var existingSteps = new HashSet<string>();

			foreach (var stepNode in descriptionElement)
			{
				var descriptionNode = stepNode.SelectSingleNode(".//div[contains(@class, 'cooking-steps-main__text')]");

				if (descriptionNode == null)
					continue;

				var descriptionText = DecodeHtml(descriptionNode.InnerText.Trim());
				var cleanedText = UseRegex(descriptionText);

				if (!existingSteps.Contains(cleanedText))
				{
					steps.Add(cleanedText);
					existingSteps.Add(cleanedText);
				}
			}


			return string.Join(" | ", steps);
		}
		public static async Task ProcessRecipeAsync(IWebDriver driver, string recipeLink, List<Recipe> recipes)
		{
			var recipe = new Recipe();

			driver.Navigate().GoToUrl(recipeLink);

			var httpClient = new HttpClient();
			var html = await httpClient.GetStringAsync(recipeLink);

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);

			SetRecipeTitle(htmlDocument, recipe);
			SetRecipePreamble(htmlDocument, recipe);
			SetRecipeImageUrl(htmlDocument, recipe);
			SetRecipeIngredients(htmlDocument, recipe);
			SetRecipeDescription(htmlDocument, recipe);

			recipes.Add(recipe);

			await Task.Delay(1000);
		}
		private static void SetRecipeTitle(HtmlDocument htmlDocument, Recipe recipe)
		{
			var titleElements = htmlDocument.DocumentNode.SelectSingleNode("//h1[contains(@class, 'recipe-header__title') or contains(@class, 'recipe-header__title--long-words')]");
			recipe.Title = titleElements?.InnerText.Trim();
		}

		private static void SetRecipePreamble(HtmlDocument htmlDocument, Recipe recipe)
		{
			var preamElements = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'recipe-header__preamble')]");
			recipe.Preamble = preamElements?.InnerText.Trim();
		}

		private static void SetRecipeImageUrl(HtmlDocument htmlDocument, Recipe recipe)
		{
			var imgElement = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'recipe-header__desktop-image-wrapper__inner')]//img");
			recipe.ImgURL = imgElement?.GetAttributeValue("src", "");
		}

		private static void SetRecipeIngredients(HtmlDocument htmlDocument, Recipe recipe)
		{
			var ingredientElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@id, 'ingredients')]//div[contains(@class, 'ingredients-list-group')]");

			if (ingredientElements == null)
				return;

			var ingredients = FilterIngredients(ingredientElements);
			recipe.Ingredients = ingredients;
		}

		private static void SetRecipeDescription(HtmlDocument htmlDocument, Recipe recipe)
		{
			var descElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@id, 'steps')]//div[contains(@class, 'cooking-steps-group')]//div");

			if (descElements == null)
				return;

			var descriptions = FilterDescription(descElements);
			recipe.Description = descriptions;
		}
	}
}

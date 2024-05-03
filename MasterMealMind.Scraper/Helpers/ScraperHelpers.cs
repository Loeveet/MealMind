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
			var timerPattern = @"Öppna timer: \d+ min(uter)?(?: \d+ sek)?";
			var dotPattern = @"\.(?!\s)";
			var nutritionPattern = @"Näringsvärde.*";
			var newDesc = Regex.Replace(description, timerPattern, "");
			newDesc = Regex.Replace(newDesc, dotPattern, ". ");
			newDesc = Regex.Replace(newDesc, nutritionPattern, "");

			return newDesc;

		}

		public static string FilterIngredients(HtmlNodeCollection ingredientElements)
		{


			List<string> ingredients = [];

			foreach (var ingredientElement in ingredientElements)
			{
				var ingredientText = DecodeHtml(ingredientElement.InnerText.Trim());
				ingredientText = UseRegex(ingredientText);
				if (!string.IsNullOrWhiteSpace(ingredientText))
				{
					ingredients.Add(DecodeHtml(ingredientText));
				}
			}

			ingredients = ingredients.Distinct().ToList();
			if (ingredients.Count > 0)
			{
				ingredients.RemoveAt(0);
			}

			return string.Join("| ", ingredients);

			//List<string> ingre = [];

			//foreach (var ingredientElement in ingredientElements)
			//{
			//	var ingredientText = DecodeHtml(ingredientElement.InnerText.Trim());
			//	ingredientText = UseRegex(ingredientText);
			//	if (!string.IsNullOrWhiteSpace(ingredientText))
			//	{
			//		ingre.Add(DecodeHtml(ingredientText));
			//	}
			//}
			//for (int i = 0; i < ingre.Count; i++)
			//{
			//	for (int j = 0; j < ingre.Count; j++)
			//	{
			//		if (i != j && ingre[i].Contains(ingre[j]))
			//		{
			//			ingre.RemoveAt(i);
			//			i--;
			//			break;
			//		}
			//	}
			//}

			//return ingre;
		}
		public static async Task ProcessRecipeAsync(IWebDriver driver, string recipeLink, List<Recipe> recipes)
		{
			var recipe = new Recipe();
			driver.Navigate().GoToUrl(recipeLink);
			var httpClient = new HttpClient();
			var html = await httpClient.GetStringAsync(recipeLink);

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);

			var titleElements = htmlDocument.DocumentNode.SelectSingleNode("//h1[contains(@class, 'recipe-header__title') or contains(@class, 'recipe-header__title--long-words')]");
			var title = titleElements.InnerText.Trim();
			recipe.Title = DecodeHtml(title);

			var preamElements = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'recipe-header__preamble')]");
			var pream = preamElements.InnerText.Trim();
			recipe.Preamble = DecodeHtml(pream);

			var imgElement = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'recipe-header__desktop-image-wrapper__inner')]//img");
			if (imgElement != null)
			{
				var imgUrl = imgElement.GetAttributeValue("src", "");
				recipe.ImgURL = DecodeHtml(imgUrl);
			}
			else
			{
				recipe.ImgURL = null; // Lägg till sån här felhantering på alla proppar
			}

			var ingredientElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@id, 'ingredients')]//div[contains(@class, 'ingredients-list-group')]//div");

			if (ingredientElements != null)
			{
				string ingredients = FilterIngredients(ingredientElements);
				//recipe.Ingredients?.AddRange(ingredients);
				recipe.Ingredients = string.Join(", ", ingredients);
			}

			var descElements = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@id, 'steps')]//div[contains(@class, 'cooking-steps-group')]//div");
			recipe.Description = DecodeHtml(descElements.InnerText.Trim());
			recipe.Description = UseRegex(recipe.Description);

			recipes.Add(recipe);
			await Task.Delay(1000);
		}
	}
}

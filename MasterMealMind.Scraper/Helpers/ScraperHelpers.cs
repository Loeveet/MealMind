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
		/*
		DecodeHtml takes a string encoded with HTML characters and converts 
		them into a readable format.
		 
		UseRegex takes a string and removes 'Öppna timer' and 'Näringsvärden', 
		as well as adds a space after periods.

		FilterIngredients takes a collection of HTML nodes representing ingredients. 
		It iterates over each node, extracts the inner text, and applies decoding 
		and regex operations to clean up the text. After cleaning, it removes any 
		duplicate ingredients and removes the first element (assumed to be a summary or title). 
		Finally, it joins the cleaned ingredients into a pipe-separated string and returns it.
		 
		ProcessRecipeAsync asynchronously processes a recipe by scraping data from a given recipe link. 
		It takes an instance of a WebDriver (IWebDriver), the URL of the recipe, and a list of recipes to populate.

		1. It navigates the WebDriver to the provided recipe link and retrieves the HTML content using an HttpClient.
		2. It parses the HTML content using the HtmlAgilityPack.
		3. It extracts various recipe details such as title, preamble, image URL, ingredients, 
		and description from specific HTML elements.
		4. It cleans up the extracted data using helper methods like DecodeHtml and UseRegex.
		5. It constructs a Recipe object with the extracted details.
		6. It adds the constructed recipe to the list of recipes.
		7. It introduces a delay of 1000 milliseconds (1 second) before continuing execution asynchronously.

		Overall, this method is responsible for scraping recipe data from a web page and populating a 
		list of recipe objects asynchronously.
		 */
		public static string DecodeHtml(string html)
		{
			return WebUtility.HtmlDecode(html);
		}

		public static string UseRegex(string description)
		{
			//	var timerPattern = @"Öppna timer: \d+ min(uter)?(?: \d+ sek)?";
			//	var dotPattern = @"\.(?!\s)";
			//	var nutritionPattern = @"Näringsvärde.*";
			//	var newDesc = Regex.Replace(description, timerPattern, string.Empty);
			//	newDesc = Regex.Replace(newDesc, dotPattern, ". ");
			//	newDesc = Regex.Replace(newDesc, nutritionPattern, string.Empty);

			//	return newDesc;

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

		public static string FilterIngredients(HtmlNodeCollection ingredientElements)
		{
			var ingredients = new List<string>();

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

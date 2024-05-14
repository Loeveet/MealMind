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

			var combinedList = new List<string>();

			if (ingredientGroups != null)
			{
				foreach (var group in ingredientGroups)
				{
					var headerNode = group.SelectSingleNode(".//h3[contains(@class, 'ingredients-list-group__heading')]");
					var ingredientNodes = group.SelectNodes(".//div[contains(@class, 'ingredients-list-group__card')]");

					if (headerNode != null)
					{
						var headerText = DecodeHtml(headerNode.InnerText.Trim());
						combinedList.Add("*" + headerText);
					}

					if (ingredientNodes != null)
					{
						var ingredientsText = ingredientNodes
						.Select(ing =>
						{
							var quantityElement = ing.SelectSingleNode(".//span[contains(@class, 'ingredients-list-group__card__qty')]");
							var ingredientNameElement = ing.SelectSingleNode(".//span[contains(@class, 'ingredients-list-group__card__ingr')]");

							if (ingredientNameElement != null)
							{
								string quantity = "";
								if (quantityElement != null)
								{
									quantity = DecodeHtml(quantityElement.InnerText.Trim());
								}

								var ingredientName = DecodeHtml(ingredientNameElement.InnerText.Trim());
								var ingredientText = string.IsNullOrWhiteSpace(quantity) ? ingredientName : $"{quantity} {ingredientName}";
								return UseRegex(ingredientText);
							}
							return null;
						})
						.ToList();

						combinedList.AddRange(ingredientsText);
					}
				}
			}

			string combinedIngredients = string.Join("| ", combinedList);
			return combinedIngredients;

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

			//Navigate to recipeLink
			driver.Navigate().GoToUrl(recipeLink);

			//Get HTML-content from recipelink
			var httpClient = new HttpClient();
			var html = await httpClient.GetStringAsync(recipeLink);

			//Create HTML-document and load HTML using HtmlAgilityPack
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);

			//Extracting recipedata
			SetRecipeTitle(htmlDocument, recipe);
			SetRecipePreamble(htmlDocument, recipe);
			SetRecipeImageUrl(htmlDocument, recipe);
			SetRecipeIngredients(htmlDocument, recipe);
			SetRecipeDescription(htmlDocument, recipe);

			recipes.Add(recipe);

			//Add delay for handle websitedelay and avoid overload on website
			await Task.Delay(2000);
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

			if (ingredientElements != null)
			{
				string ingredients = FilterIngredients(ingredientElements);
				recipe.Ingredients = ingredients;
			}
		}

		private static void SetRecipeDescription(HtmlDocument htmlDocument, Recipe recipe)
		{
			var descElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@id, 'steps')]//div[contains(@class, 'cooking-steps-group')]//div");

			if (descElements != null)
			{
				var descriptions = FilterDescription(descElements);
				recipe.Description = descriptions;
			}
		}
	}
}

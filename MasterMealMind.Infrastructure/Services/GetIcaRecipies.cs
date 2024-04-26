﻿using OpenQA.Selenium.Support.UI;
using HtmlAgilityPack;
using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;
using System.Text.RegularExpressions;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;

namespace MasterMealMind.Infrastructure.Services
{
    public class GetIcaRecipies : IGetIcaRecipies
    {
        public async Task<List<Recipe>> GetAsync()
        {
            var recipes = new List<Recipe>();

            var options = new ChromeOptions();
            options.AddArgument("--headless");

            // Instansiera Chrome WebDriver
            using (var driver = new ChromeDriver(options))
            {
                // Navigera till webbplatsen med recept
                driver.Navigate().GoToUrl("https://www.ica.se/recept/");
				var actions = new Actions(driver);

				// Lista för att lagra recept
				var recipeLinks = new List<string>();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Hitta och klicka på knappen för att acceptera cookies
                try
                {
					var acceptCookiesButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(text(), 'Godkänn kakor')]")));
					acceptCookiesButton.Click();
				}
				catch (NoSuchElementException)
				{
					Console.WriteLine("Cookiespopup-knappen hittades inte.");
					// Fortsätt med resten av koden om cookiespopup-knappen inte hittas
				}

				// Hitta knappen "Visa mer" och klicka på den för att ladda in fler recept
				int clickCount = 0;
                int maxClicks = 5; // Ange det önskade antalet klick

                while (clickCount < maxClicks)
                {
					try
					{
						var showMoreButton = driver.FindElement(By.XPath("//button[contains(@class, 'ids-button ids-button--tertiary ids-button--md ids-button--text-icon ids-button--text-icon--right')]"));
						((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", showMoreButton);
                        actions.MoveToElement(showMoreButton).Click().Perform();
                        clickCount++;
                        Thread.Sleep(1000);
					}
					catch (NoSuchElementException)
					{
						// Inga fler "Visa mer"-knappar hittades, avbryt loopen
						break;
					}
					
                }

                // Hitta länkarna till varje enskilt recept efter att alla sidor har laddats
                var recipeElements = driver.FindElements(By.XPath("//a[contains(@class, 'recipe-card__content__title font-rubrik-2--mid')]"));
                foreach (var element in recipeElements)
                {
                    recipeLinks.Add(element.GetAttribute("href"));
                }

                // Skrapa varje recept i flera trådar
                await Task.WhenAll(recipeLinks.Select(link => ProcessRecipeAsync(driver, link, recipes)));

                return recipes;

            }
        }

        static async Task ProcessRecipeAsync(IWebDriver driver, string recipeLink, List<Recipe> recipes)
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
                List<string> ingredients = FilterIngredients(ingredientElements);
                recipe.Ingredients?.AddRange(ingredients);
            }

            var descElements = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@id, 'steps')]//div[contains(@class, 'cooking-steps-group')]//div");
            recipe.Description = DecodeHtml(descElements.InnerText.Trim());
            recipe.Description = UseRegex(recipe.Description);

			recipes.Add(recipe);
            await Task.Delay(1000); 


		}
        static string UseRegex(string description)
        {
            var timerPattern = @"Öppna timer: \d+ min(uter)?(?: \d+ sek)?";

            var dotPattern = @"\.(?!\s)";

            var newDesc = Regex.Replace(description, timerPattern, "");

            newDesc = Regex.Replace(newDesc, dotPattern, ". ");

            return newDesc;

        }

        static List<string> FilterIngredients(HtmlNodeCollection ingredientElements)
        {
            List<string> ingre = [];

            foreach (var ingredientElement in ingredientElements)
            {
                var ingredientText = DecodeHtml(ingredientElement.InnerText.Trim());
				ingredientText = UseRegex(ingredientText);
				if (!string.IsNullOrWhiteSpace(ingredientText))
                {
                    ingre.Add(DecodeHtml(ingredientText));
                }
            }
            for (int i = 0; i < ingre.Count; i++)
            {
                for (int j = 0; j < ingre.Count; j++)
                {
                    if (i != j && ingre[i].Contains(ingre[j]))
                    {
                        ingre.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            return ingre;
        }
        static string DecodeHtml(string html)
        {
            return WebUtility.HtmlDecode(html);
        }
    }

}


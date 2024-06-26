﻿using MasterMealMind.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using MasterMealMind.Scraper.Helpers;
using MasterMealMind.Core.Interfaces;

namespace MasterMealMind.Scraper.Scrapers
{
	public class ICAscraper : IICAScraper
	{
		public async Task<List<Recipe>> StartScraperAsync(string url)
		{
			var recipes = new List<Recipe>();
			var options = new ChromeOptions();
			options.AddArgument("--headless");

			using (var driver = new ChromeDriver(options))
			{

				driver.Navigate().GoToUrl(url);
				var actions = new Actions(driver);

				var recipeLinks = new List<string>();

				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

				try
				{
					var acceptCookiesButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(text(), 'Godkänn kakor')]")));
					acceptCookiesButton.Click();
				}
				catch (NoSuchElementException)
				{
					Console.WriteLine("Cookiespopup-button not found.");
				}

				var clickCount = 0;
				var wantedClicks = 2;

				while (clickCount < wantedClicks)
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
						break;
					}

				}
				var recipeElements = driver.FindElements(By.XPath("//a[contains(@class, 'recipe-card__content__title font-rubrik-2--mid')]"));
				foreach (var element in recipeElements)
				{
					recipeLinks.Add(element.GetAttribute("href"));
				}

				await Task.WhenAll(recipeLinks.Select(link => ScraperHelpers.ProcessRecipeAsync(driver, link, recipes)));
			}

			return recipes;
		}
	}
}

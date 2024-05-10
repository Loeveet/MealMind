using HtmlAgilityPack;
using MasterMealMind.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using MasterMealMind.Scraper.Helpers;

namespace MasterMealMind.Scraper.Scrapers
{
    public class ICAscraper
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
                    Console.WriteLine("Cookiespopup-button not found.");
                }

                int clickCount = 0;
                int maxClicks = 0; // Ange det önskade antalet klick

                while (clickCount < maxClicks)
                {
                    try
                    {
                        var showMoreButton = driver.FindElement(By.XPath("//button[contains(@class, 'ids-button ids-button--tertiary ids-button--md ids-button--text-icon ids-button--text-icon--right')]"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", showMoreButton);
                        actions.MoveToElement(showMoreButton).Click().Perform();
                        clickCount++;
                        Thread.Sleep(2000);
                    }
                    catch (NoSuchElementException)
                    {
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
                await Task.WhenAll(recipeLinks.Select(link => ScraperHelpers.ProcessRecipeAsync(driver, link, recipes)));

                return recipes;
            }
        }
    }
}

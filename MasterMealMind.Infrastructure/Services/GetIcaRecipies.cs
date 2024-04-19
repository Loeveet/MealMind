using HtmlAgilityPack;
using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Net;
using System.Text.RegularExpressions;

namespace MasterMealMind.Infrastructure.Services
{
    public class GetIcaRecipies : IGetIcaRecipies
    {
        public async Task<List<Recipe>> Get()
        {
            var recipes = new List<Recipe>();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");

            // Instansiera Chrome WebDriver
            using (var driver = new ChromeDriver(options))
            {
                // Navigera till webbplatsen med recept
                driver.Navigate().GoToUrl("https://www.ica.se/recept/");

                // Lista för att lagra recept
                var recipeLinks = new List<string>();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                // Hitta och klicka på knappen för att acceptera cookies
                try
                {
                    System.Threading.Thread.Sleep(2000);
                    var acceptCookiesButton = driver.FindElement(By.XPath("//button[contains(text(), 'Godkänn kakor')]"));
                    acceptCookiesButton.Click();
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Cookiespopup-knappen hittades inte.");
                    // Fortsätt med resten av koden om cookiespopup-knappen inte hittas
                }

                // Hitta knappen "Visa mer" och klicka på den för att ladda in fler recept
                int clickCount = 0;
                int maxClicks = 0; // Ange det önskade antalet klick

                while (clickCount < maxClicks)
                {
                    try
                    {
                        var showMoreButton = wait.Until(driver =>
                        {
                            var element = driver.FindElement(By.XPath("//button[contains(@class, 'ids-button ids-button--tertiary ids-button--md ids-button--text-icon ids-button--text-icon--right')]"));
                            if (element != null && element.Displayed)
                            {
                                // Scrolla till knappen "Visa mer"
                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", element);


                                return element;
                            }
                            else
                            {
                                return null;
                            }
                        });
                        System.Threading.Thread.Sleep(2000); // Anpassa väntetiden efter behov
                        showMoreButton.Click();
                        System.Threading.Thread.Sleep(2000);
                        // Vänta en kort stund för att låta sidan ladda in fler recept
                        clickCount++;
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


            var ingredientElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@id, 'ingredients')]//div[contains(@class, 'ingredients-list-group')]//div");

            if (ingredientElements != null)
            {
                List<string> ingredients = FilterIngredients(ingredientElements);
                recipe.Ingredients?.AddRange(ingredients);
            }

            var descElements = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@id, 'steps')]//div[contains(@class, 'cooking-steps-group')]//div");
            recipe.Desc = DecodeHtml(descElements.InnerText.Trim());
            recipe.Desc = UseRegex(recipe.Desc);

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


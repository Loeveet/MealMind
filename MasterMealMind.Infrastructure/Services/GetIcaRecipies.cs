using HtmlAgilityPack;
using MasterMealMind.Core.Interfaces;
using MasterMealMind.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Services
{
    public class GetIcaRecipies : IGetIcaRecipies
    {
        public async Task Get()
        {
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
                await Task.WhenAll(recipeLinks.Select(link => ProcessRecipeAsync(driver, link)));

            }
        }

        static async Task ProcessRecipeAsync(IWebDriver driver, string recipeLink)
        {
            var recipe = new Recipe();
            // Navigera till receptsidan
            driver.Navigate().GoToUrl(recipeLink);
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(recipeLink).Result;

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            //Get the title
            var titleElements = htmlDocument.DocumentNode.SelectSingleNode("//h1[contains(@class, 'recipe-header__title') or contains(@class, 'recipe-header__title--long-words')]");


            var title = titleElements.InnerText.Trim();
            //Console.WriteLine("Titel: " + title);
            recipe.Title = title;


            var ingredientElements = htmlDocument.DocumentNode.SelectNodes("//div[contains(@id, 'ingredients')]//div[contains(@class, 'ingredients-list-group')]//div");


            if (ingredientElements != null)
            {
                // Skapa en lista för att lagra ingredienserna
                var ingredientsList = new List<string>();

                // Loopa igenom ingredienselementen och extrahera texten
                foreach (var ingredientElement in ingredientElements)
                {
                    // Extrahera texten från ingredienselementet och lägg till i listan
                    string ingredientText = ingredientElement.InnerText.Trim();
                    // Kontrollera om ingrediensen är tom eller inte
                    if (!string.IsNullOrWhiteSpace(ingredientText))
                    {
                        // Om ingrediensen inte är tom, lägg till den i listan
                        ingredientsList.Add(ingredientText);
                        recipe.Ingredients.Add(ingredientText);
                    }
                }

                // Skriv ut eller bearbeta listan med ingredienser här
                //foreach (var ingredient in ingredientsList)
                //{
                //    Console.WriteLine("Ingrediens: " + ingredient);
                //}
            }

            //Get the description

            var descElements = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@id, 'steps')]//div[contains(@class, 'cooking-steps-group')]//div");

            var desc = descElements.InnerText.Trim();
            recipe.Desc = desc;

            // Här kan du skrapa och behandla informationen från receptsidan
            // Till exempel, extrahera titel, ingredienser och instruktioner för att göra receptet
            // och spara den i en databas eller annan lagringsmekanism
            //Console.WriteLine($"Processing recipe: {driver.Title}");
            Console.WriteLine(recipe.Title);
            foreach (var i in recipe.Ingredients)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine(recipe.Desc);

            await Task.Delay(1000); // Lägg till en kort väntetid för att undvika att överbelasta webbplatsen

        }
    }
    class Recipe
    {
        public string? Title { get; set; }
        public string? Desc { get; set; }
        public List<string>? Ingredients { get; set; } = new List<string>();
    }
}


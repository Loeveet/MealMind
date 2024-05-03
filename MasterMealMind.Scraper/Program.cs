using MasterMealMind.Scraper.Scrapers;

namespace MasterMealMind.Scraper
{
	public class Program
	{
		public static async Task Main(string[] args)
		{

			var s = new ICAscraper();
			await s.GetAsync();
		}
	}
}

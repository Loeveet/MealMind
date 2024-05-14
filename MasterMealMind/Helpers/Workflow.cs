using MasterMealMind.Core.Interfaces;

namespace MasterMealMind.Web.Helpers
{
	public class Workflow(IGetIcaRecipies getIcaRecipies)
	{
		private readonly IGetIcaRecipies _getIcaRecipies = getIcaRecipies;

		public async void RunScraper()
		{
			await _getIcaRecipies.GetIcaAsync();
		}
	}
}

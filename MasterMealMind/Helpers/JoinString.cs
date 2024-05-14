using MasterMealMind.Core.Models;

namespace MasterMealMind.Web.Helpers
{
	public static class JoinString
	{
		public static string JoinData(string[] data, string separator)
		{
			return string.Join(separator, data);
		}
	}
}

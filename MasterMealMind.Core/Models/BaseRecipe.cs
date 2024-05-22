using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Models
{
	public class BaseRecipe
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Preamble { get; set; }
		public string? ImgURL { get; set; }
		public string? Ingredients { get; set; }
	}
}

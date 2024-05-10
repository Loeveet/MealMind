using MasterMealMind.Core.Enum;

namespace MasterMealMind.Core.Models
{
    public class Grocery
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Quantity { get; set; }
        public string? Description { get; set; }
        public UnitType? Unit { get; set; }
        public Storage? Storage { get; set; }

    }
}

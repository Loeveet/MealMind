using MasterMealMind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Services
{
    public class CheckInputService : ICheckInputService
    {
        public bool IsInputLengthValid(string input, int maxNumberOfDigits)
        {
            return !string.IsNullOrEmpty(input) && input.Length <= maxNumberOfDigits;
        }
    }
}

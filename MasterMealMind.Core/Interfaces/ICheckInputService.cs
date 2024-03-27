using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Interfaces
{
    public interface ICheckInputService
    {
        bool IsInputLengthValid(string input, int maxNumberOfDigits);
    }
}

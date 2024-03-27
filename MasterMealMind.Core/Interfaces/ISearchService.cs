using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Core.Interfaces
{
    public interface ISearchService
    {
        string GetSearchString();
        void SetSearchString(string searchString);
        void ClearSearchString();
    }
}

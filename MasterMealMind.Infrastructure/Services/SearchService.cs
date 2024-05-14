using MasterMealMind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private static string? _searchString;

        public string GetSearchString() => _searchString ??= string.Empty;
        public void SetSearchString(string searchString) => _searchString = searchString;
        public void ClearSearchString() => _searchString = string.Empty;

    }
}

using MasterMealMind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MasterMealMind.Infrastructure.Services
{
    public class ProfanityFilterService : IProfanityFilterService
    {
        public string FilterProfanity(string input, int timeoutMilliseconds)
        {
            string[] badWords = new string[] { "jävla", "jävel", "satan", "fanskap", "idiot", "skit" };
            string filteredText = input;

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                cancellationTokenSource.CancelAfter(timeoutMilliseconds);

                var task = Task.Run(() =>
                {
                    if (input != null)
                    {
                        foreach (string word in badWords)
                        {
                            string pattern = $"({word})";

                            filteredText = Regex.Replace(filteredText, pattern, "****", RegexOptions.IgnoreCase | RegexOptions.NonBacktracking);
                        }
                    }
                }, cancellationTokenSource.Token);

                try
                {
                    task.Wait(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    throw new TaskCanceledException();
                }
            }

            return filteredText;
        }
    }
}

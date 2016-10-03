using System;
using System.Collections.Generic;
using System.Linq;

namespace EventCommandExample.Tests
{
    public static class Utils
    {
        public static IEnumerable<int> AllIndexesOf(this string str, char searchstring)
        {
            int minIndex = str.IndexOf(searchstring);

            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + 1);
            }
        }

        public static bool IsSequential(int[] array)
        {
            return array.Zip(array.Skip(1), (a, b) => (a + 1) == b).All(x => x);
        }

        public static IEnumerable<IEnumerable<int>> ConsecutiveSequences(IEnumerable<int> input, int minLength = 1)
        {
            var results = new List<List<int>>();

            foreach (var i in input.OrderBy(x => x))
            {
                var existing = results.FirstOrDefault(lst => lst.Last() + 1 == i);

                if (existing == null)
                    results.Add(new List<int> { i });
                else
                    existing.Add(i);
            }

            var result = minLength <= 1 ? results :
                results.Where(lst => lst.Count >= minLength);

            return result;
        }
    }
}

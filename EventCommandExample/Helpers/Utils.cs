using System;
using System.Collections.Generic;
using System.Linq;

namespace EventCommandExample
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

        public static IEnumerable<IEnumerable<int>> HighlightText(string text, string searchString, int minLength = 1)
        {
            var listOfIndex = new List<int>();

            if (text.ToLower().Contains(searchString.ToLower()))
            {
                if (searchString.ToCharArray().Length > 0)
                {
                    foreach (var item in searchString.ToLower())
                    {
                        listOfIndex.AddRange(text.ToLower().AllIndexesOf(item));
                    }
                }
                else
                    listOfIndex.Add(text.ToLower().IndexOf(searchString.ToLower(), StringComparison.CurrentCultureIgnoreCase));
            }

            if (searchString.ToCharArray().Length > 1 &&
                searchString.ToCharArray().Length < listOfIndex.Count())
            {
                var copyOfOriginalList = listOfIndex;
                var listOrdered = listOfIndex.OrderBy(x => x).ToList();

                listOrdered.RemoveAt(0);
                listOfIndex = listOrdered;

                if (ConsecutiveSequences(listOfIndex, minLength).Count() > 0)
                    return ConsecutiveSequences(listOfIndex, minLength);

                listOrdered = copyOfOriginalList.OrderByDescending(x => x).ToList();
                listOrdered.RemoveAt(0);
                listOfIndex = listOrdered.OrderBy(x => x).ToList();

                if (ConsecutiveSequences(listOfIndex, minLength).Count() > 0)
                    return ConsecutiveSequences(listOfIndex, minLength);

                return null;
            }

            if (searchString.ToCharArray().Length == 1)
            {
                var results = new List<List<int>>();
                results.Add(listOfIndex);

                return results;
            }

            if (IsSequential(listOfIndex.OrderBy(x => x).ToArray()))
                return ConsecutiveSequences(listOfIndex, minLength);

            return null;
        }
    }
}

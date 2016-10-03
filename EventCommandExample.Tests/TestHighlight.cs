using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;

namespace EventCommandExample.Tests
{
    [TestFixture]
    public class TestHighlight
    {
        [Test]
        public void Test_Function_For_HighlightIndex()
        {
            var text = "Rodrigo Amaro dos Santos Amaral";
            var searchString = "am";
            var listOfIndex = new List<int>();

            if (text.ToLower().Contains(searchString.ToLower()))
            {
                if (searchString.ToCharArray().Length > 0)
                {
                    foreach (var item in searchString)
                    {
                        listOfIndex.AddRange(text.ToLower().AllIndexesOf(item));
                    }
                }
                else
                    listOfIndex.Add(text.ToLower().IndexOf(searchString.ToLower(), StringComparison.InvariantCultureIgnoreCase));
            }

            if (searchString.ToCharArray().Length < listOfIndex.Count())
            {
                var copyOfOriginalList = listOfIndex;
                var listOrdered = listOfIndex.OrderBy(x => x).ToList();

                listOrdered.RemoveAt(0);
                listOfIndex = listOrdered;

                if (Utils.ConsecutiveSequences(listOfIndex, 3).Count() > 0)
                    Assert.IsNotNull(listOfIndex);
                else
                {
                    listOrdered = copyOfOriginalList.OrderByDescending(x => x).ToList();
                    listOrdered.RemoveAt(0);
                    listOfIndex = listOrdered.OrderBy(x => x).ToList();

                    if (Utils.ConsecutiveSequences(listOfIndex, 3).Count() > 0)
                        Assert.IsNotNull(listOfIndex);
                    else
                        Assert.Fail();
                }
            }
            else
            {
                if (Utils.IsSequential(listOfIndex.ToArray()))
                    Assert.IsNotNull(listOfIndex);
                else
                    Assert.Fail();
            }
        }
    }
}

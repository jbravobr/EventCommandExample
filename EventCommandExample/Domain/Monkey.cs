using System;
using System.Linq;
using Xamarin.Forms;

namespace EventCommandExample
{
    public class Monkey
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }

        //URL for our monkey image!
        public string Image { get; set; }

        public string NameSort => Name[0].ToString();

        public FormattedString NameFormatted { get; set; }
    }
}


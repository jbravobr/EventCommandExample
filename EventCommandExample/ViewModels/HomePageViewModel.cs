using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventCommandExample.ViewModels
{
    public class HomePageViewModel : BindableBase
    {
        public string Title { get; } = "Hello World !";

        public HomePageViewModel()
        {
        }
    }
}


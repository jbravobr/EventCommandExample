using System;
using Prism.Mvvm;
using Prism.Navigation;
using PropertyChanged;

namespace EventCommandExample.ViewModels
{
    [ImplementPropertyChanged]
    public class DetailsPageViewModel : BindableBase, INavigationAware
    {
        public Monkey Monkey { get; set; }

        public DetailsPageViewModel()
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("monkey"))
                Monkey = (Monkey)parameters["monkey"];
        }
    }
}


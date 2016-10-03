using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;
using System.Collections.Generic;
using System;

namespace EventCommandExample.ViewModels
{
    [ImplementPropertyChanged]
    public class MonkeyListPageViewModel : BindableBase
    {
        public ObservableCollection<Monkey> Monkeys { get; set; }
        public ObservableCollection<Grouping<string, Monkey>> MonkeysGrouped { get; set; }
        public FormattedString searchedMonkeyNameFormatted { get; set; }

        public bool isSearching { get; set; }
        public bool isNotSearching { get; set; } = true;

        INavigationService _navigationService;

        public MonkeyListPageViewModel(INavigationService navigationService)
        {
            LoadMonkeysToViewModel();
            _navigationService = navigationService;
        }

        public int MonkeyCount => Monkeys.Count;

        public Command GetMonkeyDetail
        {
            get
            {
                return new Command<Monkey>(async (monkey) => await NavigateToDetails(monkey));
            }
        }

        public Command SearchMonkey
        {
            get
            {
                isNotSearching = false;
                isSearching = true;

                return new Command<string>(DoSearchMonkey);
            }
        }

        void DoSearchMonkey(string letter)
        {
            if (string.IsNullOrEmpty(letter))
                LoadMonkeysToViewModel();

            var sorted = from monkey in Monkeys
                         where monkey.Name.ToLowerInvariant().Contains(letter.ToLowerInvariant())
                         orderby monkey.Name
                         group monkey by monkey.NameSort into monkeyGroup
                         select new Grouping<string, Monkey>(monkeyGroup.Key, monkeyGroup);

            if (!string.IsNullOrEmpty(letter))
                foreach (var item in sorted)
                {
                    item.FirstOrDefault().NameFormatted = HighlightMonkeyNameSearched(item.First().Name, letter);
                }

            MonkeysGrouped = new ObservableCollection<Grouping<string, Monkey>>(sorted);
        }

        void LoadMonkeysToViewModel()
        {
            Monkeys = MonkeyHelper.Monkeys;
            MonkeysGrouped = MonkeyHelper.MonkeysGrouped;
        }

        async Task NavigateToDetails(Monkey monkey)
        {
            var parameters = new NavigationParameters();
            parameters.Add("monkey", monkey);

            await _navigationService.NavigateAsync("DetailsPage", parameters);
        }

        public FormattedString HighlightMonkeyNameSearched(string name, string letters, int minLength = 1)
        {
            var indexes = Utils.HighlightText(name, letters, minLength);
            var formattedString = new FormattedString();

            foreach (var item in indexes)
            {
                for (int i = 0; i < name.Length; i++)
                {
                    if (!item.Contains(i))
                        formattedString.Spans.Add(new Span
                        {
                            Text = name[i].ToString()
                        });
                    else if (item.Contains(i))
                        formattedString.Spans.Add(new Span
                        {
                            Text = name[i].ToString(),
                            FontAttributes = FontAttributes.Bold | FontAttributes.Italic
                        });
                }
            }

            return formattedString;
        }
    }
}


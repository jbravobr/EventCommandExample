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
using System.Text;
using Humanizer;

namespace EventCommandExample.ViewModels
{
    [ImplementPropertyChanged]
    public class MonkeyListPageViewModel : BindableBase
    {
        public ObservableCollection<Monkey> Monkeys { get; set; }
        public ObservableCollection<Grouping<string, Monkey>> MonkeysGrouped { get; set; }
        public FormattedString searchedMonkeyNameFormatted { get; set; }

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
            else
                foreach (var item in sorted)
                {
                    var formattedString = new FormattedString();
                    formattedString.Spans.Add(new Span { Text = item.First().Name });
                    item.FirstOrDefault().NameFormatted = formattedString;
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
            var formattedString = new FormattedString();
            var lettersAdded = new List<string>();


            if (!lettersAdded.Any(letters.Contains))
            {
                lettersAdded.Add(letters);
                var str = new StringBuilder(name.ToLower());

                var index = name.ToLower().IndexOf(letters.ToLower());
                var removed = str.Remove(name.ToLower().IndexOf(letters.ToLower()), letters.Length);

                if (index == 0)
                {
                    formattedString.Spans.Add(new Span
                    {
                        Text = letters.Transform(To.TitleCase),
                        FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                        ForegroundColor = Color.Red
                    });
                    formattedString.Spans.Add(new Span
                    {
                        Text = removed.ToString()
                    });
                }
                else if (index > 0)
                {
                    bool isBeginOfName = true;

                    for (int i = 0; i < name.Length; i++)
                    {
                        if (i < index)
                        {
                            if (isBeginOfName)
                            {
                                formattedString.Spans.Add(new Span
                                {
                                    Text = removed[i].ToString().Transform(To.TitleCase)
                                });

                                isBeginOfName = false;
                            }
                            else
                                formattedString.Spans.Add(new Span
                                {
                                    Text = removed[i].ToString().Transform(To.LowerCase)
                                });
                        }
                        else if (i == index)
                        {
                            if (char.IsWhiteSpace(name[i - 1]))
                                formattedString.Spans.Add(new Span
                                {
                                    Text = letters.Transform(To.TitleCase),
                                    FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                                    ForegroundColor = Color.Red
                                });
                            else
                                formattedString.Spans.Add(new Span
                                {
                                    Text = letters.Transform(To.LowerCase),
                                    FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                                    ForegroundColor = Color.Red
                                });
                        }
                        else
                        {
                            if (letters.Length > 1)
                            {
                                var formattedText = string.Empty;
                                foreach (var span in formattedString.Spans)
                                {
                                    formattedText += span.Text;
                                }

                                if (i - 1 < name.Length)
                                {
                                    if (formattedText.ToLower() != name.ToLower())
                                    {
                                        if (char.IsWhiteSpace(name[i - 1]))
                                            formattedString.Spans.Add(new Span
                                            {
                                                Text = name[i + letters.Length - 1].ToString().Transform(To.TitleCase)
                                            });
                                        else
                                            formattedString.Spans.Add(new Span
                                            {
                                                Text = name[i + letters.Length - 1].ToString()
                                            });
                                    }
                                }
                            }
                            else
                            {
                                if (char.IsWhiteSpace(name[i - 1]))
                                    formattedString.Spans.Add(new Span
                                    {
                                        Text = name[i].ToString().Transform(To.TitleCase)
                                    });
                                else
                                    formattedString.Spans.Add(new Span
                                    {
                                        Text = name[i].ToString()
                                    });
                            }
                        }
                    }
                }
            }

            return formattedString;
        }
    }
}


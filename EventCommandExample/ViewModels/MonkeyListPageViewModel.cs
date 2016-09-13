using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;

namespace EventCommandExample.ViewModels
{
    [ImplementPropertyChanged]
    public class MonkeyListPageViewModel : BindableBase
    {
        public ObservableCollection<Monkey> Monkeys { get; set; }
        public ObservableCollection<Grouping<string, Monkey>> MonkeysGrouped { get; set; }

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
    }
}


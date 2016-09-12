using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;

namespace EventCommandExample.ViewModels
{
    public class MonkeyListPageViewModel : BindableBase
    {
        public ObservableCollection<Monkey> Monkeys { get; set; }
        public ObservableCollection<Grouping<string, Monkey>> MonkeysGrouped { get; set; }

        INavigationService _navigationService;

        public MonkeyListPageViewModel(INavigationService navigationService)
        {
            Monkeys = MonkeyHelper.Monkeys;
            MonkeysGrouped = MonkeyHelper.MonkeysGrouped;
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

        async Task NavigateToDetails(Monkey monkey)
        {
            var parameters = new NavigationParameters();
            parameters.Add("monkey", monkey);

            await _navigationService.NavigateAsync("DetailsPage", parameters);
        }
    }
}


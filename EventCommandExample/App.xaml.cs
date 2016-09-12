using EventCommandExample.Views;
using Prism.Unity;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EventCommandExample
{
    public partial class App : PrismApplication
    {
        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync("MonkeyListPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MonkeyListPage>();
            Container.RegisterTypeForNavigation<DetailsPage>();
            Container.RegisterTypeForNavigation<HomePage>();
        }
    }
}


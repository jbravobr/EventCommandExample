using Xamarin.Forms;

namespace EventCommandExample.Views
{
    public partial class MonkeyListPage : ContentPage
    {
        public MonkeyListPage()
        {
            InitializeComponent();

            listViewMonkeys.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }
    }
}



using System.Threading.Tasks;
using Xamarin.Forms;

namespace WPPodUser
{
    public partial class BannerView : ContentPage
    {
        public BannerView(string text)
        {
            InitializeComponent();

            Message.Text = text;
        }

        public string Text
        {
            set
            {
                Message.Text = value;
            }
        }

        public async Task Show(INavigation nav)
        {
            await nav.PushModalAsync(this);
            IsBusy = true;
        }

        public async Task Hide()
        {
            IsBusy = false;
            await Navigation.PopModalAsync();
        }
    }
}

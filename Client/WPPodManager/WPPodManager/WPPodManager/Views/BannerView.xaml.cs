using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPPodManager.Views
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

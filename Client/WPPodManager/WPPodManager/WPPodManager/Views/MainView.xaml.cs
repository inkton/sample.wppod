using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Inkton.Nester;
using Inkton.Nester.Models;
using Inkton.Nester.ViewModels;
using Inkton.Nester.Cloud;
using Inkton.Nester.Cache;
using WPPodManager.Views;
using WPPodManager.ViewModels;

namespace WPPodManager.Views
{
    public partial class MainView : TabbedPage
    {
        public MainView()
        {
            InitializeComponent();

            Children.Add(new NavigationPage(new AboutPage())
            {
                Title = "About",
                Icon = Device.OnPlatform("tab_about.png", null, null)
            });

            Children.Add(new NavigationPage(new MenuPage())
            {
                Title = "Food Menu",
                Icon = Device.OnPlatform("tab_feed.png", null, null)
            });
        }

        public INesterControl NesterControl
        {
            get
            {
                return (App.Current as INesterControl);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var loadingPage = new BannerView("Plase wait ...");

                AuthViewModel authViewModel = new AuthViewModel();

                Permit permit = new Permit();
                permit.Owner = NesterControl.User;
                permit.Password = "helloworld";

                authViewModel.Permit = permit;
                ServerStatus status = await authViewModel.QueryTokenAsync(false);

                if (status.Code != 0)
                {
                    await DisplayAlert("WP Pod", "Failed to authorize the app", "OK");
                }
                else
                {
                    AppViewModel appViewModel = new AppViewModel();
                    Inkton.Nester.Models.App wppApp = new Inkton.Nester.Models.App();
                    wppApp.Tag = "wppod";

                    status = await appViewModel.QueryAppAsync(wppApp, false);

                    if (status.Code != 0)
                    {
                        await DisplayAlert("WP Pod", "Failed to make a connection to the app", "OK");
                    }

                    _appModelPair = new AppModelPair(
                        authViewModel, appViewModel);
                }
            }
            catch (Exception ex)
            {
                await MainPage.DisplayAlert("Nester", ex.Message, "OK");
            }
        }
    }
}

using System;
using System.Resources;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Inkton.Nester;
using Inkton.Nester.Models;
using Inkton.Nester.ViewModels;
using Inkton.Nester.Cloud;
using Inkton.Nester.Cache;
using WPPodManager.Views;

namespace WPPodManager
{
    public partial class App : Application, INesterControl
    {
        private User _user;
        private const int ServiceVersion = 1;
        private NesterService _service, _target;
        private StorageService _storage;
        private BaseModels _baseModels;
        private Permit _permit;

        private BannerView _progressView;
        private AboutPage _aboutPage;
        private MenuPage _menuPage;
        private OrderPage _orderPage;
        private StockPage _stockPage;

        public App()
        {
            InitializeComponent();

            _user = new User();

            _service = new NesterService();
            _service.Version = ServiceVersion;

            _target = new NesterService();
            _storage = new StorageService();
            _storage.Clear();

            _baseModels = new BaseModels(
                new AuthViewModel(),
                new PaymentViewModel(),
                new AppViewModel());

            _aboutPage = new AboutPage();
            _menuPage = new MenuPage();
            _orderPage = new OrderPage();
            _stockPage = new StockPage();

            _progressView = new BannerView("Please wait ..");
            _aboutPage = new AboutPage();
            _menuPage = new MenuPage();
            _orderPage = new OrderPage();
            _stockPage = new StockPage();

            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(_progressView)
                    {
                        Title = "Starting"
                    }
                }
            };

            Device.BeginInvokeOnMainThread(
                async () => await InitAsync());
        }

        public BaseModels BaseModels
        {
            get { return _baseModels; }
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public AppViewModel Target
        {
            get { return _baseModels.TargetViewModel; }
            set
            {
                _baseModels.TargetViewModel = value;

                if (value != null)
                {
                    _target.Version = ServiceVersion;
                    _target.Endpoint = string.Format(
                        "https://{0}/", value.EditApp.Hostname);
                    _target.BasicAuth = new BasicAuth(
                        true, value.EditApp.Tag, 
                        value.EditApp.NetworkPassword);
                }
            }
        }

        public NesterService Service
        {
            get { return _service; }
        }

        public NesterService DeployedApp
        {
            get { return _target; }
        }

        public StorageService StorageService
        {
            get { return _storage; }
        }

        public string StoragePath
        {
            get
            {
                return System.IO.Path.GetTempPath();
            }
        }

        public ResourceManager GetResourceManager()
        {
            ResourceManager resmgr = new ResourceManager(
                "WPPodManager.Properties.Resources",
                typeof(App).GetTypeInfo().Assembly);
            return resmgr;
        }

        public async Task ResetViewAsync(AppViewModel appModel = null)
        {
            AppViewModel targetModel = appModel != null ?                 
                appModel : _baseModels.TargetViewModel;

            Inkton.Nester.Models.App wppodApp = new Inkton.Nester.Models.App();
            wppodApp.Owner = _user;
            wppodApp.Tag = "wppod";
            targetModel.EditApp = wppodApp;
            await targetModel.InitAsync();
            Target = targetModel;

            await _aboutPage.SetupAsync();

            _menuPage.App = wppodApp;
            _orderPage.App = wppodApp;
            _stockPage.App = wppodApp;
        }

        private async Task InitAsync()
        {
            try
            {
                // the app owner/admin
                _user = new User();
                _user.Email = "john@nest.yt";

                _permit = new Permit();
                _permit.Owner = User;
                _permit.Password = "helloworld";

                _baseModels.AuthViewModel.Permit = _permit;
                ServerStatus status = await _baseModels.AuthViewModel
                    .QueryTokenAsync(false);

                if (status.Code != 0)
                {
                    _progressView.Text = "Failed to authorize the app";
                }
                else
                {
                    await _baseModels.PaymentViewModel.InitAsync();

                    await ResetViewAsync();

                    (Current.MainPage as TabbedPage).Children.Clear();

                    (Current.MainPage as TabbedPage).Children.Add(
                        new NavigationPage(_aboutPage)
                        {
                            Title = "About"
                        }
                    );

                    (Current.MainPage as TabbedPage).Children.Add(
                        new NavigationPage(_menuPage)
                        {
                            Title = "Cafe Menu"
                        }
                    );

                    (Current.MainPage as TabbedPage).Children.Add(
                        new NavigationPage(_orderPage)
                        {
                            Title = "Orders"
                        }
                    );

                    (Current.MainPage as TabbedPage).Children.Add(
                        new NavigationPage(_stockPage)
                        {
                            Title = "Stocks"
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                await MainPage.DisplayAlert("Nester", ex.Message, "OK");
            }
        }
    }
}

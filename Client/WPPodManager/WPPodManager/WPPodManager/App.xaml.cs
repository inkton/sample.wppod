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
        private const int ServiceVersion = 1;
        private NesterService _appMeta, _appDeployed;
        private StorageService _storage;

        private BaseModels _baseModels;
        private User _user;
        private Permit _permit;

        private BannerView _progressView;
        private AboutPage _aboutPage;
        private MenuPage _menuPage;
        private OrderPage _orderPage;
        private StockPage _stockPage;

        public App ()
		{
			InitializeComponent();

            _baseModels = new BaseModels(
                    new AuthViewModel(),
                    new PaymentViewModel(),
                    new AppViewModel());

            _user = new User();

            _appMeta = new NesterService();
            _appMeta.Version = ServiceVersion;

            _appDeployed = new NesterService();
            _appDeployed.Version = ServiceVersion;

            _storage = new StorageService();
            _storage.Clear();

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

        public NesterService Service
        {
            get { return _appMeta; }
        }

        public NesterService DeployedApp
        {
            get { return _appDeployed; }
        }

        public AppViewModel Target
        {
            get { return _baseModels.TargetViewModel; }
            set
            {
                _baseModels.TargetViewModel = value;

                if (value != null)
                {
                    _appDeployed.Version = ServiceVersion;
                    _appDeployed.Endpoint = string.Format(
                        "https://{0}/", value.EditApp.Hostname);
                    _appDeployed.BasicAuth = new BasicAuth(
                        true, value.EditApp.Tag,
                        value.EditApp.NetworkPassword);
                }
            }
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
            wppodApp.Tag = WPPod.Platform.AppTag;
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
                _user.Email = WPPod.Platform.Email;

                _permit = new Permit();
                _permit.Owner = User;
                _permit.Password = WPPod.Platform.Password;

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

using System;
using System.Resources;
using System.Reflection;
using Xamarin.Forms;
using Inkton.Nester;
using Inkton.Nester.Models;
using Inkton.Nester.ViewModels;
using Inkton.Nester.Cloud;
using Inkton.Nester.Cache;
using WPPodManager.Views;
using System.Threading.Tasks;

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
        private AboutPage _aboutPage;

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
            SetMainPage(_aboutPage);
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

            _aboutPage.Setup();
            await _aboutPage.GetAnalyticsAsync();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            try
            {
                // the app owner/admin
                _user = new User();
                _user.Email = "john@nest.yt";

                _permit = new Permit();
                _permit.Owner = User;
                _permit.Password = "helloworld";

                var loadingPage = new BannerView("Starting ..");
                await loadingPage.Show(MainPage.Navigation);

                _baseModels.AuthViewModel.Permit = _permit;
                ServerStatus status = await _baseModels.AuthViewModel
                    .QueryTokenAsync(false);

                if (status.Code != 0)
                {
                    loadingPage.Text = "Failed to authorize the app";
                }
                else
                {
                    await _baseModels.PaymentViewModel.InitAsync();

                    await ResetViewAsync();

                    await loadingPage.Hide();
                }
            }
            catch (Exception ex)
            {
                await MainPage.DisplayAlert("Nester", ex.Message, "OK");
            }
        }

        public static void SetMainPage(AboutPage aboutPage)
        {
            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(aboutPage)
                    {
                        Title = "About",
                        //Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                    new NavigationPage(new MenuPage())
                    {
                        Title = "Cafe Menu",
                        //Icon = Device.OnPlatform("tab_feed.png",null,null)
                    }
                }
            };
        }
    }
}

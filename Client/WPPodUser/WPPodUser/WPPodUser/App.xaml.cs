using System;
using System.Resources;
using System.Reflection;
using Xamarin.Forms;
using Inkton.Nester;
using Inkton.Nester.Models;
using Inkton.Nester.ViewModels;
using Inkton.Nester.Cloud;
using Inkton.Nester.Cache;
using System.Threading.Tasks;

namespace WPPodUser
{
    public partial class App : Application, INesterControl
    {
        private User _user;
        private const int ServiceVersion = 1;
        private NesterService _service, _target;
        private StorageService _storage;
        private BaseModels _baseModels;
        private Permit _permit;

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
            MainPage = new NavigationPage(
                new WPPodUser.LoginView());
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
            wppodApp.Tag = WPPod.Platform.AppTag;
            targetModel.EditApp = wppodApp;

            await targetModel.InitAsync();
            Target = targetModel;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            try
            {
                // the app owner/admin
                _user = new User();
                _user.Email = WPPod.Platform.Email;

                _permit = new Permit();
                _permit.Owner = User;
                _permit.Password = WPPod.Platform.Password;

                var loadingPage = new BannerView("Please wait ...");
                await loadingPage.Show(MainPage.Navigation);

                _baseModels.AuthViewModel.Permit = _permit;
                ServerStatus status = await _baseModels.
                    AuthViewModel.QueryTokenAsync();

                await ResetViewAsync();

                await loadingPage.Hide();
            }
            catch (Exception ex)
            {
                await MainPage.DisplayAlert("Nester", ex.Message, "OK");
            }
        }
    }
}

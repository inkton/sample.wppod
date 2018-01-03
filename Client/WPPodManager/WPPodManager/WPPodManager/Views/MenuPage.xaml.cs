using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using WPPodManager.ViewModels;

namespace WPPodManager.Views
{
    public partial class MenuPage : ContentPage
    {
        private MenuViewModel _viewModel;
        private Inkton.Nester.Models.App _app;

        public MenuPage()
        {
            InitializeComponent();

            Menu.SelectedIndexChanged += Menu_SelectedIndexChangedAsync;

            _viewModel = new MenuViewModel();
            BindingContext = _viewModel;
        }

        public Inkton.Nester.Models.App App
        {
            get { return _app; }
            set { _app = value; }
        }

        public async Task SetupAsync()
        {
            _viewModel.EditApp = _app;

            await _viewModel.LoadMenusAsync();
        }

        private async void Menu_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            await _viewModel.LoadMenuItemsAsync();
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuItemPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(
                async () => await SetupAsync());
        }
    }
}

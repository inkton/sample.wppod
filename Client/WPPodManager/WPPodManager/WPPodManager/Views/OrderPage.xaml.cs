using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using WPPodManager.ViewModels;

namespace WPPodManager.Views
{
    public partial class OrderPage : ContentPage
    {
        private OrderViewModel _viewModel;
        private Inkton.Nester.Models.App _app;

        public OrderPage()
        {
            InitializeComponent();

            SelectedDate.Date = DateTime.Now;
            SelectedDate.PropertyChanged += SelectedDate_PropertyChangedAsync;

            Refresh.Clicked += Refresh_ClickedAsync;

            _viewModel = new OrderViewModel();
            BindingContext = _viewModel;
        }

        public async Task SetupAsync()
        {
            _viewModel.EditApp = _app;

            await _viewModel.LoadOrdersAsync(DateTime.Now);
        }

        public Inkton.Nester.Models.App App
        {
            get { return _app; }
            set { _app = value; }
        }

        private async void SelectedDate_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await _viewModel.LoadOrdersAsync(SelectedDate.Date);
        }

        private async void Refresh_ClickedAsync(object sender, EventArgs e)
        {
            await _viewModel.LoadOrdersAsync(SelectedDate.Date);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(
                async () => await SetupAsync());
        }
    }
}

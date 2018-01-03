using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using WPPodManager.ViewModels;

namespace WPPodManager.Views
{
    public partial class StockPage : ContentPage
    {
        private StockViewModel _viewModel;
        private Inkton.Nester.Models.App _app;

        public StockPage()
        {
            InitializeComponent();

            SelectedDate.Date = DateTime.Now;
            SelectedDate.PropertyChanged += SelectedDate_PropertyChangedAsync;

            Refresh.Clicked += Refresh_ClickedAsync;

            _viewModel = new StockViewModel();
            BindingContext = _viewModel;
        }

        public async Task SetupAsync()
        {
            _viewModel.EditApp = _app;

            if (_viewModel.Stocks.Count == 0)
            {
                await _viewModel.LoadStocksAsync();
            }

            await _viewModel.LoadStockItemsByDateAsync(DateTime.Now); 
        }

        public Inkton.Nester.Models.App App
        {
            get { return _app; }
            set { _app = value; }
        }

        private async void SelectedDate_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await _viewModel.LoadStockItemsByDateAsync(SelectedDate.Date);
        }

        private async void Refresh_ClickedAsync(object sender, EventArgs e)
        {
            await _viewModel.LoadStockItemsByDateAsync(SelectedDate.Date);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(
                async () => await SetupAsync());
        }
    }
}

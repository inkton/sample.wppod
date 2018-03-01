using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WPPodUser.ViewModels;

namespace WPPodUser
{
    public partial class MainPage : ContentPage
    {
        private OrderViewModel _viewModel;

        public MainPage(OrderViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            BindingContext = _viewModel;

            Menu.SelectedIndexChanged += Menu_SelectedIndexChangedAsync;
            AddItem.Clicked += AddItem_Clicked;
            Submit.Clicked += Submit_ClickedAsync;

            OrderDatePicker.Date = DateTime.Today;
        }

        private async void Menu_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            await _viewModel.LoadMenuItemsAsync();
        }

        private void AddItem_Clicked(object sender, EventArgs e)
        {
            int serves = 0;
            if (!int.TryParse(Serves.Text, out serves))
            {
                DisplayAlert("Alert", "Enter a valid number of serves", "OK");
            }
            else
            {
                _viewModel.AddOrderItem(serves);
            }
        }

        private async void Submit_ClickedAsync(object sender, EventArgs e)
        {
            await _viewModel.SubmitOrderAsync();
            await DisplayAlert("Alert", "Order submitted", "OK");
        }
    }
}

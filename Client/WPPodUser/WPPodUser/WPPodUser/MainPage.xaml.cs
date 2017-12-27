using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WPPod.ViewModels;

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
        }
        private async void Menu_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            await _viewModel.LoadMenuItemsAsync();
        }
    }
}

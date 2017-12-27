using System;

using WPPodManager.Models;
using WPPodManager.ViewModels;

using Xamarin.Forms;

namespace WPPodManager.Views
{
    public partial class MenuPage : ContentPage
    {
        private MenuViewModel _viewModel;

        public MenuPage()
        {
            InitializeComponent();

            _viewModel = new MenuViewModel();

            BindingContext = _viewModel;

            Menu.SelectedIndex = 0;
            Menu.SelectedIndexChanged += Menu_SelectedIndexChanged;
        }

        private void Menu_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewModel.LoadMenuItemsCommand.Execute(null);
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuItemPage());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Menu.Items.Clear();

            await _viewModel.LoadMenusAsync();

            foreach (Models.Menu menu in _viewModel.EditMenus)
            {
                Menu.Items.Add(menu.Name);
            }

            await _viewModel.LoadMenuItemsAsync();
        }
    }
}

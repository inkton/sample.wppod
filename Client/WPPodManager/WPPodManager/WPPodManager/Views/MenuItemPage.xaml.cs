using System;

using WPPodManager.Models;

using Xamarin.Forms;

namespace WPPodManager.Views
{
    public partial class MenuItemPage : ContentPage
    {
        public Models.MenuItem Item { get; set; }

        public MenuItemPage()
        {
            InitializeComponent();

            Item = new Models.MenuItem
            {
                Title = "Menu Item title",
                Description = "This is a nice description",
                Price = 0,
                Photo = "path to menu item image"
            };

            BindingContext = this;

            FoodType.SelectedIndex = 0;
        }

        public string[] FoodTypes
        {
            get
            {
                return Enum.GetNames(typeof(FoodType));
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Item.FoodType = (FoodType)Enum.Parse(typeof(FoodType), 
                FoodType.SelectedItem as string);
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
using System;
using Xamarin.Forms;
using WPPod.Models;

namespace WPPodManager.Views
{
    public partial class MenuItemPage : ContentPage
    {
        public WPPod.Models.MenuItem Item { get; set; }

        public MenuItemPage()
        {
            InitializeComponent();

            Item = new WPPod.Models.MenuItem
            {
                Title = "Menu Item title",
                Description = "This is a nice description",
                Price = 0,
                PhotoURL = "path to menu item image"
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using WPPod.Models;
using WPPodUser.ViewModels;
using WPPod.ViewModels;

namespace WPPodUser
{
    public partial class LoginView : ContentPage
    {
        // From here https://www.codeproject.com/Articles/22777/Email-Address-Validation-Using-Regular-Expression
        public const string MatchEmailPattern =
                  @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
           + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        public LoginView()
        {
            InitializeComponent();
            BindingContext = new UserViewModel();
        }

        public static bool IsValidEmail(string email)
        {
            if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
            else return false;
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {

            try
            {
                if (!IsValidEmail(EmailEntry.Text))
                    throw new Exception();

                var user = new User
                {
                    Email = EmailEntry.Text,
                    Pin = PinEntry.Text
                };

                OrderViewModel ordersViewModel = new OrderViewModel();
                await ordersViewModel.LoadMenusAsync();
                Navigation.InsertPageBefore(new WPPodUser.MainPage(ordersViewModel), this);
                await Navigation.PopAsync();
            }
            catch 
            {
                messageLabel.Text = "Login failed";
                PinEntry.Text = string.Empty;
            }
        }
    }
}

using System;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Inkton.Nester;
using Inkton.Nester.Models;
using System.Threading.Tasks;
using Inkton.Nester.ViewModels;

namespace WPPodManager.Views
{
    public partial class AboutPage : ContentPage
    {
        private INesterControl _nesterControl;

        public AboutPage()
        {
            InitializeComponent();

            Fetch.Clicked += Fetch_ClickedAsync;
            _nesterControl = (Application.Current as INesterControl);
        }

        public async Task SetupAsync()
        {
            BindingContext = _nesterControl.BaseModels.TargetViewModel;

            await GetAnalyticsAsync();
        }

        private async void Fetch_ClickedAsync(object sender, EventArgs e)
        {
            await GetAnalyticsAsync();
        }

        public async Task GetAnalyticsAsync()
        {
            try
            {
                // Show events during the last hour
                int hoursToCheck;
                if (!int.TryParse(Hours.Text, out hoursToCheck))
                {
                    await DisplayAlert("Nester", "Pleas enter a valid number of hours",
                        _nesterControl.BaseModels.TargetViewModel.EditApp.Name, "OK");
                }

                DateTime unixEpoch = new DateTime(1970, 1, 1);
                DateTime pastTrackInHours = DateTime.Now.ToUniversalTime().AddHours(-1 * hoursToCheck);

                long unixEpochSinceHourAgo = (long)(pastTrackInHours - unixEpoch).TotalSeconds;

                _nesterControl.BaseModels
                    .TargetViewModel.LogViewModel.QueryIndexs =
                            LogViewModel.QueryIndex.QueryIndexNestLog |
                            LogViewModel.QueryIndex.QueryIndexDiskSpace |
                            LogViewModel.QueryIndex.QueryIndexCpu;

                await _nesterControl.BaseModels.TargetViewModel
                    .LogViewModel.QueryAsync(unixEpochSinceHourAgo);

                if ((NestLogs.ItemsSource as ObservableCollection<NestLog>).Any())
                {
                    NestLogs.SelectedItem = 
                        (NestLogs.ItemsSource as ObservableCollection<NestLog>).Last();
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Nester", "Failed to retrieve metrics from the app because\n" + e.Message,
                    _nesterControl.BaseModels.TargetViewModel.EditApp.Name, "OK");
            }
        }
    }
}

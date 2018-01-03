using System;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Inkton.Nester;
using Inkton.Nester.Models;
using System.Threading.Tasks;

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
            _nesterControl.BaseModels.TargetViewModel.LogViewModel.SetupDiskSpaceSeries(
                DiskSpaceData.Series[0]
                );

            _nesterControl.BaseModels.TargetViewModel.LogViewModel.SetupCPUSeries(
                CpuData.Series[0],
                CpuData.Series[1],
                CpuData.Series[2],
                CpuData.Series[3],
                CpuData.Series[4]
                );

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

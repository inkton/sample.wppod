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

            Refresh.Clicked += Refresh_ClickedAsync;
        }

        private async void Refresh_ClickedAsync(object sender, EventArgs e)
        {
            await GetAnalyticsAsync();
        }

        public void Setup()
        {
            _nesterControl = (Application.Current as INesterControl);

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
        }

        public async Task GetAnalyticsAsync()
        {
            try
            {
                /*
                DateTime unixEpoch = new DateTime(1970, 1, 1);
                DateTime analyzeDateUTC = DateTime.Now.ToUniversalTime();
                DateTime startTime = analyzeDateUTC - new TimeSpan(5, 0, 0);
                DateTime endTime = analyzeDateUTC + new TimeSpan(1, 0, 0);

                await _nesterControl.BaseModels.TargetViewModel.LogViewModel.QueryMetricsAsync(
                    (long)(startTime - unixEpoch).TotalMilliseconds,
                    (long)(endTime - unixEpoch).TotalMilliseconds);
                */

                await _nesterControl.BaseModels.TargetViewModel
                    .LogViewModel.QueryAsync(true, 50);
                NestLogs.SelectedItem = (NestLogs.ItemsSource as ObservableCollection<NestLog>).Last();
            }
            catch (Exception e)
            {
                await DisplayAlert("Nester", "Failed to retrieve metrics from the app because\n" + e.Message,
                    _nesterControl.BaseModels.TargetViewModel.EditApp.Name, "OK");
            }
        }
    }
}

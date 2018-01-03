using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Inkton.Nester.Cloud;

namespace WPPodManager.ViewModels
{
    public class OrderViewModel : Inkton.Nester.ViewModels.ViewModel
    {
        private ObservableCollection<WPPod.Models.Order> _orders;
        private WPPod.Models.Order _selectedOrder;

        public OrderViewModel(Inkton.Nester.Models.App app = null)
            : base(app)
        {
            _orders = new ObservableCollection<WPPod.Models.Order>();
        }

        public ObservableCollection<WPPod.Models.Order> Orders
        {
            get
            {
                return _orders;
            }
        }

        public WPPod.Models.Order SelectedOrder
        {
            get
            {
                return _selectedOrder;
            }
            set
            {
                SetProperty(ref _selectedOrder, value);
            }
        }

        async public Task LoadOrdersAsync(DateTime date)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                WPPod.Models.Order orderSeed = new WPPod.Models.Order();

                Dictionary<string, string> filter = new Dictionary<string, string>();
                filter["date"] = date.ToString("d");

                ServerStatus status = await ResultMultiple<WPPod.Models.Order>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, orderSeed, false, filter);

                if (status.Code >= 0)
                {
                    _orders = status.PayloadToList<WPPod.Models.Order>();
                    OnPropertyChanged("Orders");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

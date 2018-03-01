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
                filter["date"] = date.ToString("yyyy-MM-dd");

                ServerStatus status = await ResultMultiple<WPPod.Models.Order>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, orderSeed, false, filter);

                if (status.Code >= 0)
                {
                    _orders = status.PayloadToList<WPPod.Models.Order>();
                    WPPod.Models.OrderItem itemSeed = new WPPod.Models.OrderItem();

                    foreach (var order in _orders)
                    {
                        itemSeed.Order = order;

                        status = await ResultMultiple<WPPod.Models.OrderItem>.WaitForObjectAsync(
                            NesterControl.DeployedApp, true, itemSeed);

                        if (status.Code >= 0)
                        {
                            order.Items = status.PayloadToList<WPPod.Models.OrderItem>();
                            WPPod.Models.MenuItem menuItemSeed = new WPPod.Models.MenuItem();

                            foreach (var orderItem in order.Items)
                            {
                                menuItemSeed.Menu = new WPPod.Models.Menu();
                                menuItemSeed.Menu.Id = orderItem.MenuId;
                                menuItemSeed.Id = orderItem.MenuItemId;

                                status = await ResultSingle<WPPod.Models.MenuItem>.WaitForObjectAsync(
                                    true, menuItemSeed, new CachedHttpRequest<WPPod.Models.MenuItem>(
                                        NesterControl.DeployedApp.QueryAsync), false, null, null);

                                orderItem.MenuItem = status.PayloadToObject<WPPod.Models.MenuItem>();
                            }
                        }
                    }

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

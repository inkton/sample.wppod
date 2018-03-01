using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using Inkton.Nester.Cloud;
using WPPod.Models;

namespace WPPodUser.ViewModels
{
    public class OrderViewModel : Inkton.Nester.ViewModels.ViewModel
    {
        private User _user;
        private Order _order;

        private ObservableCollection<WPPod.Models.Menu> _menus;
        private WPPod.Models.Menu _selectedMenu;

        private ObservableCollection<WPPod.Models.MenuItem> _menuItems;
        private WPPod.Models.MenuItem _selectedMenuItem;

        public OrderViewModel(Inkton.Nester.Models.App app = null)
            : base(app)
        {
            _order = new Order();
            _menus = new ObservableCollection<WPPod.Models.Menu>();
            _menuItems = new ObservableCollection<WPPod.Models.MenuItem>();
        }

        public DateTime MinimumDate
        {
            get { return DateTime.Now; }
        }

        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                SetProperty(ref _user, value);

                _user.UseEmailAsKey = false;
            }
        }

        public Order EditOrder
        {
            get
            {
                return _order;
            }
        }

        public ObservableCollection<WPPod.Models.Menu> EditMenus
        {
            get
            {
                return _menus;
            }
        }

        public WPPod.Models.Menu SelectedMenu
        {
            get
            {
                return _selectedMenu;
            }
            set
            {
                SetProperty(ref _selectedMenu, value);
            }
        }

        public ObservableCollection<WPPod.Models.MenuItem> EditMenuItems
        {
            get
            {
                return _menuItems;
            }
        }

        public WPPod.Models.MenuItem SelectedMenuItem
        {
            get
            {
                return _selectedMenuItem;
            }
            set
            {
                SetProperty(ref _selectedMenuItem, value);
            }
        }

        public void AddOrderItem(int serves)
        {
            WPPod.Models.OrderItem orderItem = new WPPod.Models.OrderItem();
            orderItem.Menu = SelectedMenu;
            orderItem.MenuItem = SelectedMenuItem;
            orderItem.Quantity = serves;

            EditOrder.Items.Add(orderItem);
        }

        async public Task<ServerStatus> LoadMenusAsync()
        {
            ServerStatus status = new ServerStatus(
                ServerStatus.NEST_RESULT_ERROR);

            if (IsBusy)
                return status;

            IsBusy = true;

            try
            {
                WPPod.Models.Menu menuSeed = new WPPod.Models.Menu();

                status = await ResultMultiple<WPPod.Models.Menu>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, menuSeed, false);

                if (status.Code >= 0)
                {
                    _menus = status.PayloadToList<WPPod.Models.Menu>();
                    OnPropertyChanged("EditMenus");
                    SelectedMenu = _menus.FirstOrDefault();

                    IsBusy = false;
                    await LoadMenuItemsAsync();
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

            return status;
        }

        async public Task<ServerStatus> LoadMenuItemsAsync()
        {
            ServerStatus status = new ServerStatus(
                ServerStatus.NEST_RESULT_ERROR);

            if (IsBusy)
                return status;

            IsBusy = true;

            try
            {
                EditMenuItems.Clear();

                WPPod.Models.MenuItem menuItemSeed = new WPPod.Models.MenuItem();
                menuItemSeed.Menu = _selectedMenu;

                status = await ResultMultiple<WPPod.Models.MenuItem>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, menuItemSeed, false);

                if (status.Code >= 0)
                {
                    _menuItems = status.PayloadToList<WPPod.Models.MenuItem>();
                    OnPropertyChanged("EditMenuItems");
                    SelectedMenuItem = _menuItems.FirstOrDefault();
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

            return status;
        }

        async public Task<ServerStatus> SubmitOrderAsync()
        {
            ServerStatus status = new ServerStatus(
                ServerStatus.NEST_RESULT_ERROR);

            if (IsBusy)
                return status;

            IsBusy = true;

            try
            {
                _order.User = _user;

                status = await ResultSingle<WPPod.Models.Order>.WaitForObjectAsync(
                    false, _order, new CachedHttpRequest<WPPod.Models.Order>(
                        NesterControl.DeployedApp.CreateAsync), false);

                if (status.Code >= 0)
                {
                    var orderItems = _order.Items;
                    _order = status.PayloadToObject<WPPod.Models.Order>();

                    foreach (var orderItem in orderItems)
                    {
                        orderItem.Order = _order;

                        status = await ResultSingle<WPPod.Models.OrderItem>.WaitForObjectAsync(
                            false, orderItem, new CachedHttpRequest<WPPod.Models.OrderItem>(
                                NesterControl.DeployedApp.CreateAsync), false);

                        if (status.Code < 0)
                        {
                            return status;
                        }
                    }

                    EditOrder.Items.Clear();
                    OnPropertyChanged("EditOrder");
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

            return status;
        }
    }
}
 
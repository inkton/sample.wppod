using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Inkton.Nester.Cloud;

namespace WPPod.ViewModels
{
    public class OrderViewModel : Inkton.Nester.ViewModels.ViewModel
    {
        private ObservableCollection<Models.Menu> _menus;
        private Models.Menu _selectedMenu;

        private ObservableCollection<Models.MenuItem> _menuItems;
        private Models.MenuItem _selectedMenuItem;

        public OrderViewModel(Inkton.Nester.Models.App app = null)
            : base(app)
        {
            _menus = new ObservableCollection<Models.Menu>();
            _menuItems = new ObservableCollection<Models.MenuItem>();
        }

        public DateTime MinimumDate
        {
            get { return DateTime.Now; }
        }

        public Models.Order EditOrder { get; set; }

        public ObservableCollection<Models.Menu> EditMenus
        {
            get
            {
                return _menus;
            }
        }

        public Models.Menu SelectedMenu
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

        public ObservableCollection<Models.MenuItem> EditMenuItems
        {
            get
            {
                return _menuItems;
            }
        }

        public Models.MenuItem SelectedMenuItem
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

        async public Task<int> LoadMenusAsync()
        {
            if (IsBusy)
                return 0;

            IsBusy = true;

            try
            {
                Models.Menu menuSeed = new Models.Menu();

                ServerStatus status = await ResultMultiple<Models.Menu>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, menuSeed, false);

                if (status.Code >= 0)
                {
                    _menus = status.PayloadToList<Models.Menu>();
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

            return _menus.Count;
        }

        async public Task<int> LoadMenuItemsAsync()
        {
            if (IsBusy)
                return 0;

            IsBusy = true;

            try
            {
                EditMenuItems.Clear();

                Models.MenuItem menuItemSeed = new Models.MenuItem();
                menuItemSeed.Menu = _selectedMenu;
                menuItemSeed.MenuId = menuItemSeed.Menu.Id;

                ServerStatus status = await ResultMultiple<Models.MenuItem>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, menuItemSeed, false);

                if (status.Code >= 0)
                {
                    _menuItems = status.PayloadToList<Models.MenuItem>();
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

            return _menuItems.Count;
        }
    }
}
 
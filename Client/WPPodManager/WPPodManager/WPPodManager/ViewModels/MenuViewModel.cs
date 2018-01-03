using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using Inkton.Nester.Cloud;
using System.Collections.ObjectModel;

namespace WPPodManager.ViewModels
{
    public class MenuViewModel : Inkton.Nester.ViewModels.ViewModel
    {
        public ICommand LoadMenusCommand { get; private set; }
        public ICommand LoadMenuItemsCommand { get; private set; }

        private ObservableCollection<WPPod.Models.Menu> _menus;
        private WPPod.Models.Menu _selectedMenu;

        private ObservableCollection<WPPod.Models.MenuItem> _menuItems;
        private WPPod.Models.MenuItem _selectedMenuItem;

        public MenuViewModel(Inkton.Nester.Models.App app = null)
            : base(app)
        {
            _menus = new ObservableCollection<WPPod.Models.Menu>();
            _menuItems = new ObservableCollection<WPPod.Models.MenuItem>();

            MessagingCenter.Subscribe<Views.MenuItemPage, WPPod.Models.MenuItem>(this, "AddItem", async (obj, item) =>
            {
                WPPod.Models.MenuItem menuItem = item as WPPod.Models.MenuItem;
                menuItem.Id = null;
                menuItem.Menu = _selectedMenu;

                ServerStatus status = await NesterControl.DeployedApp.CreateAsync(menuItem);

                if (status.Code == 0)
                {
                    await LoadMenusAsync();
                }
            });

            LoadMenusCommand = new Command(async () => await LoadMenusAsync());
            LoadMenuItemsCommand = new Command(async () => await LoadMenuItemsAsync());
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

        async public Task LoadMenusAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                WPPod.Models.Menu menuSeed = new WPPod.Models.Menu();

                ServerStatus status = await ResultMultiple<WPPod.Models.Menu>.WaitForObjectAsync(
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
        }

        async public Task LoadMenuItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                EditMenuItems.Clear();

                WPPod.Models.MenuItem menuItemSeed = new WPPod.Models.MenuItem();
                menuItemSeed.Menu = _selectedMenu;

                ServerStatus status = await ResultMultiple<WPPod.Models.MenuItem>.WaitForObjectAsync(
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
        }
    }
}
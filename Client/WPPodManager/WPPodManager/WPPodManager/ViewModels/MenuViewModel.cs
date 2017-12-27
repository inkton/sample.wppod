using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WPPodManager.Helpers;
using Xamarin.Forms;
using Inkton.Nester.Cloud;
using System.Collections.ObjectModel;
using System.Collections.Generic;
//using WPPod.Models;

namespace WPPodManager.ViewModels
{
    public class MenuViewModel : Inkton.Nester.ViewModels.ViewModel
    {
        public ICommand LoadMenusCommand { get; private set; }
        public ICommand LoadMenuItemsCommand { get; private set; }

        private ObservableCollection<Models.Menu> _menus;
        private ObservableCollection<Models.MenuItem> _menuItems;
        private int _selectedMenuIndex;

        public MenuViewModel(Inkton.Nester.Models.App app = null)
            : base(app)
        {
            _menus = new ObservableCollection<Models.Menu>();
            _menuItems = new ObservableCollection<Models.MenuItem>();

            MessagingCenter.Subscribe<Views.MenuItemPage, Models.MenuItem>(this, "AddItem", async (obj, item) =>
            {
                Models.MenuItem menuItem = item as Models.MenuItem;
                menuItem.Id = null;
                menuItem.Menu = GetSelectedMenu();
                menuItem.MenuId = menuItem.Menu.Id;

                ServerStatus status = await NesterControl.DeployedApp.CreateAsync(menuItem);

                if (status.Code == 0)
                {
                    EditMenuItems.Add(menuItem);
                }
            });

            LoadMenusCommand = new Command(async () => await LoadMenusAsync());
            LoadMenuItemsCommand = new Command(async () => await LoadMenuItemsAsync());
        }

        public ObservableCollection<Models.Menu> EditMenus
        {
            get
            {
                return _menus;
            }
        }

        public ObservableCollection<Models.MenuItem> EditMenuItems
        {
            get
            {
                return _menuItems;
            }
        }

        public int SelectedMenuIndex
        {
            get
            {
                return _selectedMenuIndex;
            }
            set
            {
                SetProperty(ref _selectedMenuIndex, value);
            }
        }

        private Models.Menu GetSelectedMenu()
        {
            Models.Menu selectedMenu = null;
            int menuIndex = 0;

            foreach (Models.Menu menu in EditMenus)
            {
                if (menuIndex++ == _selectedMenuIndex)
                {
                    selectedMenu = menu;
                    break;
                }
            }

            return selectedMenu;
        }

        async public Task LoadMenusAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {               
                Models.Menu menuSeed = new Models.Menu();

                ServerStatus status = await ResultMultiple<Models.Menu>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, menuSeed, false);

                if (status.Code >= 0)
                {
                    _menus = status.PayloadToList<Models.Menu>();
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

                Models.MenuItem menuItemSeed = new Models.MenuItem();
                menuItemSeed.Menu = GetSelectedMenu();

                ServerStatus status = await ResultMultiple<Models.MenuItem>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, menuItemSeed, false);

                if (status.Code >= 0)
                {
                    _menuItems = status.PayloadToList<Models.MenuItem>();
                    OnPropertyChanged("EditMenuItems");
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
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using Inkton.Nester.Cloud;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace WPPodManager.ViewModels
{
    public class StockViewModel : Inkton.Nester.ViewModels.ViewModel
    {
        private ObservableCollection<WPPod.Models.Stock> _stocks;
        private ObservableCollection<WPPod.Models.StockItem> _stockItems;

        public StockViewModel(Inkton.Nester.Models.App app = null)
            : base(app)
        {
            _stocks = new ObservableCollection<WPPod.Models.Stock>();
            _stockItems = new ObservableCollection<WPPod.Models.StockItem>();
        }

        public ObservableCollection<WPPod.Models.Stock> Stocks
        {
            get
            {
                return _stocks;
            }
        }

        public ObservableCollection<WPPod.Models.StockItem> StockItems
        {
            get
            {
                return _stockItems;
            }
        }

        async public Task LoadStocksAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                WPPod.Models.Stock StockSeed = new WPPod.Models.Stock();

                ServerStatus status = await ResultMultiple<WPPod.Models.Stock>.WaitForObjectAsync(
                    NesterControl.DeployedApp, true, StockSeed, false);

                if (status.Code >= 0)
                {
                    _stocks = status.PayloadToList<WPPod.Models.Stock>();
                    OnPropertyChanged("Stocks");
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

        async public Task LoadStockItemsByDateAsync(DateTime date)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                _stockItems.Clear();
                WPPod.Models.StockItem itemSummary;

                foreach (var stock in _stocks)
                {
                    WPPod.Models.StockItem stockItemSeed = new WPPod.Models.StockItem();
                    stockItemSeed.Stock = stock;

                    Dictionary<string, string> filter = new Dictionary<string, string>();
                    filter["date"] = date.ToString("d");

                    ServerStatus status = await ResultMultiple<WPPod.Models.StockItem>.WaitForObjectAsync(
                        NesterControl.DeployedApp, true, stockItemSeed, false, filter);

                    if (status.Code >= 0)
                    {
                        var itmes = status.PayloadToList<WPPod.Models.StockItem>();
                        itemSummary = new WPPod.Models.StockItem();
                        itemSummary.Stock = stock;
                        itemSummary.Quantity = 0;

                        foreach (var calcStockItem in itmes)
                        {
                            itemSummary.Quantity += calcStockItem.Quantity;
                        }

                        if (itmes.Any())
                        {
                            itemSummary.Unit = itmes.First().Unit;
                            _stockItems.Add(itemSummary);
                        }                        
                    }
                }

                OnPropertyChanged("StockItems");
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

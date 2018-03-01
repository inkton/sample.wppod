using Newtonsoft.Json;
using System;

namespace WPPod.Models
{
    public class StockItem : Inkton.Nester.Cloud.ManagedEntity
    {
        private long? _id = null;
        private long? _stockId = null;
        private Stock _stock;
        private DateTime _timeRequired;
        private double _quantity = 0;
        private string _unit = string.Empty;

        public StockItem()
            : base("stock_item")
        {
        }

        public override string Key
        {
            get
            {
                return _id.ToString();
            }
        }

        override public string Collection
        {
            get
            {
                if (_stock != null)
                {
                    return _stock.CollectionKey + base.Collection;
                }
                else
                {
                    return base.Collection;
                }
            }
        }

        override public string CollectionKey
        {
            get
            {
                if (_stock != null)
                {
                    return _stock.CollectionKey + base.CollectionKey;
                }
                else
                {
                    return base.CollectionKey;
                }
            }
        }

        [JsonProperty("id")]
        public long? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [JsonProperty("stock_id")]
        public long? StockId
        {
            get { return _stockId; }
            set { SetProperty(ref _stockId, value); }
        }

        [JsonIgnore]
        public Stock Stock
        {
            get { return _stock; }
            set
            {
                SetProperty(ref _stock, value);
                _stockId = _stock.Id;
            }
        }

        [JsonProperty("time_required")]
        public DateTime TimeRequired
        {
            get { return _timeRequired; }
            set { SetProperty(ref _timeRequired, value); }
        }

        [JsonProperty("quantity")]
        public double Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }

        [JsonProperty("unit")]
        public string Unit
        {
            get { return _unit; }
            set { SetProperty(ref _unit, value); }
        }
    }
}

using Newtonsoft.Json;

namespace WPPod.Models
{
    public class OrderItem : Inkton.Nester.Cloud.ManagedEntity
    {
        public OrderItem()
            : base("order_item")
        {
        }

        private long? _id = null;

        [JsonProperty("id")]
        public long? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [JsonIgnore]
        public string Title
        {
            get {
                if (_item == null)
                {
                    return "<none>";
                }
                else
                {
                    return string.Format("{0} x {1} $ {2:0.00}", 
                        _item.Title, _quantity, _item.Price);
                }
            }
        }

        [JsonProperty("order_id")]
        public long? OrderId
        {
            get
            {
                if (_order != null)
                    return _order.Id;
                else
                    return null;
            }
        }

        private Order _order;

        [JsonIgnore]
        public Order Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        [JsonProperty("menu_item_id")]
        public long? MenuItemId
        {
            get
            {
                if (_item != null)
                    return _item.Id;
                else
                    return null;
            }
        }

        private MenuItem _item;

        [JsonProperty("menu_item")]
        public MenuItem MenuItem
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        private int _quantity;

        [JsonProperty("quantity")]
        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }
    }
}

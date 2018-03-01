using Newtonsoft.Json;

namespace WPPod.Models
{
    public class OrderItem : Inkton.Nester.Cloud.ManagedEntity
    {
        private long? _id = null;
        private long? _orderId = null;
        private Order _order;
        private long? _menuId = null;
        private Menu _menu;
        private long? _menuItemId = null;
        private MenuItem _item;
        private int _quantity;

        public OrderItem()
            : base("order_item")
        {
        }

        public override string Key
        {
            get { return _id.ToString(); }
        }

        override public string Collection
        {
            get
            {
                if (_order != null)
                {
                    return _order.CollectionKey + base.Collection;
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
                if (_order != null)
                {
                    return _order.CollectionKey + base.CollectionKey;
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
            get { return _orderId; }
            set { SetProperty(ref _orderId, value); }
        }

        [JsonIgnore]
        public Order Order
        {
            get { return _order; }
            set
            {
                SetProperty(ref _order, value);
                _orderId = _order.Id;
            }
        }

        [JsonProperty("menu_id")]
        public long? MenuId
        {
            get { return _menuId; }
            set { SetProperty(ref _menuId, value); }
        }

        [JsonIgnore]
        public Menu Menu
        {
            get { return _menu; }
            set
            {
                SetProperty(ref _menu, value);
                _menuId = _menu.Id;
            }
        }

        [JsonProperty("menu_item_id")]
        public long? MenuItemId
        {
            get { return _menuItemId; }
            set { SetProperty(ref _menuItemId, value); }
        }

        [JsonIgnore]
        public MenuItem MenuItem
        {
            get { return _item; }
            set
            {
                SetProperty(ref _item, value);
                _menuItemId = _item.Id;
            }
        }

        [JsonProperty("quantity")]
        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }
    }
}

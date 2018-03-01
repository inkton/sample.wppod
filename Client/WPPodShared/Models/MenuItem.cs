using Newtonsoft.Json;

namespace WPPod.Models
{
    public enum FoodType
    {
        HotBeverage,
        Sandwich,
        Salad
    }

    public class MenuItem : Inkton.Nester.Cloud.ManagedEntity
    {
        private long? _id = null;
        private Menu _menu;
        private long? menuId = null;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _photoURL = string.Empty;
        private double _price = 0;
        private FoodType _foodType = FoodType.HotBeverage;

        public MenuItem()
            : base("menu_item")
        {
        }

        public override string Key
        {
            get { return Id.ToString(); }
        }

        override public string Collection
        {
            get
            {
                if (_menu != null)
                {
                    return _menu.CollectionKey + base.Collection;
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
                if (_menu != null)
                {
                    return _menu.CollectionKey + base.CollectionKey;
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

        [JsonProperty("menu_id")]
        public long? MenuId
        {
            get { return menuId; }
            set { SetProperty(ref menuId, value); }
        }

        [JsonIgnore]
        public Menu Menu
        {
            get { return _menu; }
            set {
                SetProperty(ref _menu, value);
                menuId = _menu.Id;
            }
        }

        [JsonProperty("title")]
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        [JsonProperty("price")]
        public double Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        [JsonProperty("photo_url")]
        public string PhotoURL
        {
            get { return _photoURL; }
            set { SetProperty(ref _photoURL, value); }
        }

        [JsonProperty("food_type")]
        public FoodType FoodType
        {
            get { return _foodType; }
            set { SetProperty(ref _foodType, value); }
        }
    }
}

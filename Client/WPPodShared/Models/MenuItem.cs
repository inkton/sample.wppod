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

        private long? _id = null;

        [JsonProperty("id")]
        public long? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [JsonProperty("menu_id")]
        public long? MenuId
        {
            get {
                if (_menu != null)
                    return _menu.Id;
                else
                    return null;
            }
        }

        private Menu _menu;

        public Menu Menu
        {
            get { return _menu; }
            set { SetProperty(ref _menu, value); }
        }

        private string _title = string.Empty;

        [JsonProperty("title")]
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _description = string.Empty;

        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private double _price = 0;

        [JsonProperty("price")]
        public double Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private string _photo = string.Empty;

        [JsonProperty("photo")]
        public string Photo
        {
            get { return _photo; }
            set { SetProperty(ref _photo, value); }
        }

        private FoodType _foodType = FoodType.HotBeverage;

        [JsonProperty("food_type")]
        public FoodType FoodType
        {
            get { return _foodType; }
            set { SetProperty(ref _foodType, value); }
        }
    }
}

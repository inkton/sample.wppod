using Newtonsoft.Json;

namespace WPPodManager.Models
{
    public class Menu : Inkton.Nester.Cloud.ManagedEntity
    {
        public Menu()
            : base("menu")
        {
        }

        public override string Key
        {
            get { return _id.ToString(); }
        }

        private long _id;

        [JsonProperty("id")]
        public long Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}

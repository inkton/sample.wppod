using Newtonsoft.Json;

namespace WPPod.Models
{
    public class Stock : Inkton.Nester.Cloud.ManagedEntity
    {
        public Stock()
            : base("stock")
        {
        }

        public override string Key
        {
            get { return _id.ToString(); }
        }

        private long? _id = null;

        [JsonProperty("id")]
        public long? Id
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

using Newtonsoft.Json;

namespace WPPod.Models
{
    public class User : Inkton.Nester.Cloud.ManagedEntity
    {
        public User()
            : base("user")
        {
        }

        public bool UseEmailAsKey { set; get; }

        public override string Key
        {
            get {
                if (UseEmailAsKey)
                    return Email;
                else
                    return _id.ToString();
            }
        }

        private long? _id;

        [JsonProperty("id")]
        public long? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }
    }
}

using Newtonsoft.Json;

namespace WPPod.Models
{
    public class User : Inkton.Nester.Cloud.ManagedEntity
    {
        private long? _id;
        private string _email;
        private string _name;
        private string _pin;

        public User()
            : base("user")
        {
        }

        public bool UseEmailAsKey { set; get; }

        public override string Key
        {
            get
            {
                if (UseEmailAsKey)
                    return Email;
                else
                    return _id.ToString();
            }
        }

        [JsonProperty("id")]
        public long? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [JsonProperty("email")]
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        [JsonProperty("pin")]
        public string Pin
        {
            get { return _pin; }
            set { SetProperty(ref _pin, value); }
        }
    }
}

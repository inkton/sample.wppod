using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WPPod.Models
{
    public class User
    {
        [JsonProperty("id")]
        public Int32? Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

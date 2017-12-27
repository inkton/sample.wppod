using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WPPod.Models
{
    public class Order
    {
        public Int32? Id { get; set; }

        [JsonProperty("user_id")]
        public Int32 UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public DateTime VisitDate { get; set; }

        [JsonProperty("items")]
        public ICollection<OrderItem> Items { get; set; }
    }
}
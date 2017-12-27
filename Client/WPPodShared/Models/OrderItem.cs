using System;
using Newtonsoft.Json;

namespace WPPod.Models
{
    public class OrderItem
    {
        [JsonProperty("id")]
        public Int32? Id { get; set; }

        [JsonProperty("order_id")]
        public Int32? OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }

        [JsonProperty("menu_item_id")]
        public Int32 MenuItemId { get; set; }

        [JsonIgnore]
        public MenuItem Item { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}

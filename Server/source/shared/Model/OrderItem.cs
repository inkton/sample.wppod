using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wppod.Models
{
    public class OrderItem
    {
        [JsonProperty("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Int32? Id { get; set; }

        [JsonProperty("order_id")]
        public Int32 OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }

        [JsonProperty("menu_id")]
        public Int32 MenuId { get; set; }
        
        [JsonIgnore]
        public Menu Menu { get; set; }

        [JsonProperty("menu_item_id")]
        public Int32 MenuItemId { get; set; }
        
        [JsonIgnore]
        public MenuItem MenuItem { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}

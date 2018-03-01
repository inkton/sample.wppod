using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wppod.Models
{
    public class StockItem
    {
        [JsonProperty("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Int32? Id { get; set; }

        [JsonProperty("stock_id")]
        public Int32 StockId { get; set; }  

        [JsonIgnore]
        public Stock Stock { get; set; }     

        [JsonProperty("date")]
        public DateTime TimeRequired { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("unit")]
        [StringLength(256, MinimumLength = 3), Required]
        public string Unit { get; set; }
    }    
}

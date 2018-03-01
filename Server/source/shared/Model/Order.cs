using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wppod.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Int32? Id { get; set; }

        [JsonProperty("user_id")]
        public Int32 UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonProperty("visit_date")]
        public DateTime VisitDate { get; set; }

        [JsonProperty("items")]    
        public ICollection<OrderItem> Items { get; set; }
    }   
}

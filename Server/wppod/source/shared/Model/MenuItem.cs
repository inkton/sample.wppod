using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Wppod.Models
{ 
    public enum FoodType
    {
        HotBeverage,
        Sandwich,
        Salad
    }

    public class MenuItem
    {
        [JsonProperty("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Int32? Id { get; set; }

        [JsonProperty("menu_id")]
        public Int32 MenuId { get; set; }

        [JsonIgnore]
        public Menu Menu { get; set; }        
        
        [JsonProperty("title")]
        [StringLength(256, MinimumLength = 3), Required]
        public string Title { get; set; }

        [JsonProperty("description")]
        [StringLength(256, MinimumLength = 2)]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("photo")]
        [StringLength(256, MinimumLength = 2)]
        public string Photo { get; set; }

        [JsonProperty("food_type")]
        [JsonConverter(typeof(StringEnumConverter))]        
        [DisplayFormat(NullDisplayText = "Uknown Type")]
        public FoodType? FoodType { get; set; }        
    }
}
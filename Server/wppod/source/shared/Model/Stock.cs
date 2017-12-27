using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wppod.Models
{        
    public class Stock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]         
        public Int32? Id { get; set; }
        
        [StringLength(256, MinimumLength = 3), Required]
        public string Name { get; set; }
    }
}
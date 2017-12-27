using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;
 
namespace Wppod.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Int32? Id { get; set; }

        [StringLength(256, MinimumLength = 3), Required]
        public string Email { get; set; }

        [StringLength(256, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paessler.Task.Model.Models
{
    [Table("customer")]
    public class Customer
    {
        [Key]
        public int id { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string credit_card_number { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
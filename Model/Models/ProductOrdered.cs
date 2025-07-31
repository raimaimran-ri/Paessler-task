using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Paessler.Task.Model.Models
{
    [Table("product_ordered")]
    public class ProductOrdered
    {
        [Key]
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}

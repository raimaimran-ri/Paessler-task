using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paessler.Task.Model.Models
{
    [Table("order")]
    public class Order
    {
        [Key]
        public int id { get; set; }
        public int customer_id { get; set; }
        public Customer Customer { get; set; }
        public DateTime created_at { get; set; }
        public ICollection<ProductOrdered> ProductOrdered { get; set; }
    }
}
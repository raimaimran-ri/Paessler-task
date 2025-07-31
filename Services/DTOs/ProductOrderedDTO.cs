using System;

namespace Paessler.Task.Services.DTOs
{
    public class ProductOrderedDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public float ProductAmount { get; set; }
    }
}

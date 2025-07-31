using System;
namespace Paessler.Task.Services.DTOs
{
    public class OrderDTO
    {
        public int OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceEmailAddress { get; set; }
        public string InvoiceCreditCardNumber { get; set; }
        public List<ProductOrderedDTO> ProductOrdered { get; set; }
    }
}
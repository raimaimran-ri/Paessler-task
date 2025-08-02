using System;
namespace Paessler.Task.Services.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceEmailAddress { get; set; }
        public string InvoiceCreditCardNumber { get; set; }
    }
}
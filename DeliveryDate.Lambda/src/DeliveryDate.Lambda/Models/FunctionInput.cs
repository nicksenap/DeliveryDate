using System.Collections.Generic;

namespace DeliveryDate.Lambda.Models
{
    public class FunctionInput
    {
        public List<Product> Products { get; set; }
        public string PostalNumber { get; set; }
    }
}
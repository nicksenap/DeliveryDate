using System.Collections.Generic;
using DeliveryDate.Lambda.Models;

namespace DeliveryDate.SAM.Models
{
    public class FunctionInput
    {
        public List<Product> Products { get; set; }
        public string PostalNumber { get; set; }
    }
}
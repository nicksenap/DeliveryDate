using System;

namespace DeliveryDate.SAM.Models
{
    public class DeliveryDateResponse
    { 
        public string PostalCode { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsGreenDelivery { get; set; }
    }
}
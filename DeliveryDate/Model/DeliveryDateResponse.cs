using System;

namespace DeliveryDate.Model
{
    public class DeliveryDateResponse
    { 
        public int PostalCode { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsGreenDelivery { get; set; }
    }
}
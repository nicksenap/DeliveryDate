using System;
using Newtonsoft.Json;

namespace DeliveryDate.SAM.Models
{
    public class DeliveryDateResponse
    { 
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty("deliveryDate")]
        public DateTime DeliveryDate { get; set; }
        [JsonProperty("isGreenDelivery")]
        public bool IsGreenDelivery { get; set; }
    }
}
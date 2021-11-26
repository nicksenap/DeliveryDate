using System;
using System.Collections.Generic;
using DeliveryDate.Enums;

namespace DeliveryDate.Model
{
    public class Product
    {
        public Product(Guid productId, string name, IEnumerable<DayOfWeek> deliveryDays, ProductType productType, int daysInAdvance)
        {
            ProductId = productId;
            Name = name;
            DeliveryDays = deliveryDays;
            ProductType = productType;
            DaysInAdvance = daysInAdvance;
        }

        public Product()
        {
            
        }

        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public IEnumerable<DayOfWeek> DeliveryDays { get; set; }
        public ProductType ProductType { get; set; }
        public int DaysInAdvance { get; set; }
    }
}
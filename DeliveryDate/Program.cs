// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using DeliveryDate.Enums;
using DeliveryDate.Model;

namespace DeliveryDate
{
    static class Program
    {
        private static void Main()
        {
            var product1 = new Product()
            {
                ProductId = Guid.NewGuid(),
                Name = "Ägg Frigående Inomhus 15-p S 762g",
                DeliveryDays = new List<DayOfWeek>()
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday
                },
                DaysInAdvance = 1,
                ProductType = ProductType.Normal
            };

            var product2 = new Product()
            {
                ProductId = Guid.NewGuid(),
                Name = "Grädd Ädelost 36%",
                DeliveryDays = new List<DayOfWeek>()
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Friday
                },
                DaysInAdvance = 1,
                ProductType = ProductType.Normal
            };

            var product3 = new Product()
            {
                ProductId = Guid.NewGuid(),
                Name = "Köttfria Köttbullar Frysta",
                DeliveryDays = new List<DayOfWeek>()
                {
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday
                },
                DaysInAdvance = 1,
                ProductType = ProductType.External
            };

            var products = new List<Product> { product1, product2, product3 };
            bool IsGreenDelivery(DateTime d) => d.DayOfWeek == DayOfWeek.Wednesday;
            var result = GetDeliveryDates(13756, products, IsGreenDelivery);
            Console.WriteLine("Hello, World!");
        }

        private static IEnumerable<DeliveryDateResponse> GetDeliveryDates(
            int postalCode, 
            IEnumerable<Product> products, 
            Func<DateTime, bool> isGreenDelivery)
        {
            var validWeekDays = products.Select(p => p.DeliveryDays)
                .Aggregate((acc, list) => acc.Intersect(list));
            var containExternalProduct = products.Any(product => product.ProductType == ProductType.External);
            var containTemporaryProduct = products.Any(product => product.ProductType == ProductType.Temporary);
            var daysInAdvance = Math.Max(products.Select(product => product.DaysInAdvance).Max(), 
                containExternalProduct ? Constants.EXTERNAL_PRODUCT_DAYS_IN_ADVANCE : 0);

            var startDate = DateTime.UtcNow.Date.AddDays(daysInAdvance);
            var nextSunday = DateTime.UtcNow.Date.AddDays(7 - (int) DateTime.UtcNow.Date.DayOfWeek);    
            var endDate = containTemporaryProduct ? nextSunday : DateTime.UtcNow.Date.AddDays(Constants.TWO_WEEKS);
            var deliveryDateResponse = new List<DeliveryDateResponse>();
            for (var dtm = startDate; dtm <= endDate; dtm = dtm.AddDays(1))
            {
                var isValid = validWeekDays.Contains(dtm.DayOfWeek);
                if (!isValid) continue;
                var deliveryDate = new DeliveryDateResponse()
                {
                    DeliveryDate = dtm,
                    PostalCode = postalCode,
                    IsGreenDelivery = isGreenDelivery(dtm),
                };
                deliveryDateResponse.Add(deliveryDate);
            }
            return deliveryDateResponse.OrderBy(response => !response.IsGreenDelivery).ThenBy(response => response.DeliveryDate);
        }
    }
}
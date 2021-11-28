using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using DeliveryDate.Lambda.Enums;
using DeliveryDate.Lambda.Models;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DeliveryDate.Lambda
{
    public class Function
    {
        public object FunctionHandler(object input, ILambdaContext context)
        {
            var jsonString = System.Text.Json.JsonSerializer.Serialize(input);
            var functionInput = JsonConvert.DeserializeObject<FunctionInput>(jsonString);
            bool IsGreenDelivery(DateTime d) => d.DayOfWeek == DayOfWeek.Wednesday;
            var deliveryDates = GetDeliveryDates(functionInput.PostalNumber, functionInput.Products, IsGreenDelivery);
            
            return deliveryDates;
        }
        
        private static IEnumerable<DeliveryDateResponse> GetDeliveryDates(
            string postalCode, 
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
            return deliveryDateResponse
                .OrderBy(response => !response.IsGreenDelivery).ThenBy(response => response.DeliveryDate);
        }
    }
}

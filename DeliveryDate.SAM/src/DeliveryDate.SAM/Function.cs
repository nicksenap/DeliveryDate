using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

using DeliveryDate.Lambda.Models;
using DeliveryDate.SAM.Enums;
using DeliveryDate.SAM.Models;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DeliveryDate.SAM
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");
            var body = request.Body;
            var functionInput = JsonConvert.DeserializeObject<FunctionInput>(body);
            bool IsGreenDelivery(DateTime d) => d.DayOfWeek == DayOfWeek.Wednesday;
            var result = GetDeliveryDates(functionInput.PostalNumber, functionInput.Products, IsGreenDelivery);
            var jsonResult = JsonConvert.SerializeObject(result);
            
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = jsonResult,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
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

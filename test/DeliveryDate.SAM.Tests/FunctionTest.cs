using System;
using System.Collections.Generic;
using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using DeliveryDate.Lambda.Models;
using DeliveryDate.SAM.Enums;
using DeliveryDate.SAM.Models;
using Newtonsoft.Json;

namespace DeliveryDate.SAM.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        [Fact]
        public void TestGetMethod()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            var functions = new Functions();
            var functionInput = JsonConvert.SerializeObject(FunctionInputFixture());
            const string functionOutput = "[{\"postalCode\":\"13760\",\"deliveryDate\":\"2021-12-08T00:00:00Z\",\"isGreenDelivery\":true},{\"postalCode\":\"13760\",\"deliveryDate\":\"2021-12-06T00:00:00Z\",\"isGreenDelivery\":false},{\"postalCode\":\"13760\",\"deliveryDate\":\"2021-12-10T00:00:00Z\",\"isGreenDelivery\":false},{\"postalCode\":\"13760\",\"deliveryDate\":\"2021-12-13T00:00:00Z\",\"isGreenDelivery\":false}]";
            request = new APIGatewayProxyRequest
            {
                Body = functionInput
            };

            context = new TestLambdaContext();
            response = functions.POST(request, context);
            Assert.Equal(200, response.StatusCode);
            Assert.Equal(functionOutput, response.Body);
        }
        
        private static FunctionInput FunctionInputFixture()
        {
            var product1 = new Product()
            {
                ProductId = 1,
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
                ProductId = 2,
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
                ProductId = 3,
                Name = "Köttfria Köttbullar Frysta",
                DeliveryDays = new List<DayOfWeek>()
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday
                },
                DaysInAdvance = 1,
                ProductType = ProductType.External
            };

            var products = new List<Product> { product1, product2, product3 };
            var input = new FunctionInput { Products = products, PostalNumber = "13760" };
            return input;
        }
    }
}

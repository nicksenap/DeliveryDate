using System;
using System.Collections.Generic;
using Xunit;
using Amazon.Lambda.TestUtilities;
using DeliveryDate.Lambda.Enums;
using DeliveryDate.Lambda.Models;
using Newtonsoft.Json;

namespace DeliveryDate.Lambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestLambdaFunction()
        {
            var input = FunctionInputFixture();
            const string output =
                "[{\"PostalCode\":\"13760\",\"DeliveryDate\":\"2021-12-08T00:00:00Z\",\"IsGreenDelivery\":true},{\"PostalCode\":\"13760\",\"DeliveryDate\":\"2021-12-03T00:00:00Z\",\"IsGreenDelivery\":false},{\"PostalCode\":\"13760\",\"DeliveryDate\":\"2021-12-10T00:00:00Z\",\"IsGreenDelivery\":false}]";
            var function = new Function();
            var context = new TestLambdaContext();
            var result = JsonConvert.SerializeObject(function.FunctionHandler(input, context));

            Assert.Equal(output, result);
        }
        
        [Fact]
        public void TestLambdaFunctionWithTempProduct()
        {
            var input = FunctionInputFixtureWithTempProduct();
            const string output = "[]";
            var function = new Function();
            var context = new TestLambdaContext();
            var result = JsonConvert.SerializeObject(function.FunctionHandler(input, context));

            Assert.Equal(output, result);
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
        
        private static FunctionInput FunctionInputFixtureWithTempProduct()
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
                ProductType = ProductType.Temporary
            };

            var product3 = new Product()
            {
                ProductId = 3,
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
            var input = new FunctionInput { Products = products, PostalNumber = "13760" };
            return input;
        }
    }
}

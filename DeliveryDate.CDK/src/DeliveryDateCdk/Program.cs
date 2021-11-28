using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryDateCdk
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            var deliveryDateCdkStack = new DeliveryDateCdkStack(app, "DeliveryDateCdkStack", new DeliveryDateCdkStackProps
            {
               StackName = "DeliveryDateCdkStack",
               Description = "Stack for DeliveryDate",
               
            });

            app.Synth();
        }
    }
}

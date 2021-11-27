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
            new DeliveryDateCdkStack(app, "DeliveryDateCdkStack", new DeliveryDateCdkStackProps
            {
                AnalyticsReporting = null,
                Description = null,
                Env = null,
                StackName = null,
                Synthesizer = null,
                Tags = null,
                TerminationProtection = null,
                Bucket = null
            });

            app.Synth();
        }
    }
}

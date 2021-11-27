using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.APIGateway;


namespace DeliveryDateCdk
{

    public class DeliveryDateCdkStackProps : StackProps
    {
        public IBucket Bucket { get; set; }
    }
    public class DeliveryDateCdkStack : Stack
    {
        internal DeliveryDateCdkStack(Construct scope, string id, DeliveryDateCdkStackProps props = null) : base(scope, id, props)
        {
            // The code that defines your stack goes here
            Create(props);
            
        }
        
        private void Create(DeliveryDateCdkStackProps props)
        {
            var function = CreateLambdaFunction(props);
            var apiGateway = CreateApiGateway(props, function);
            // var cloudWatchEvent = CreateCloudWatchEvent(apiCallerFunction, props);
            // var dynamoDb = CreateDynamoDb(syncFunction);
            // var stepFunction = CreateStepFunction(props, syncFunction);

            // var envName = $"Openuniverse{props.ShortName}-env";
            //
            // Amazon.CDK.Tags.Of(dynamoDb).Add("Name", envName);
            // Amazon.CDK.Tags.Of(apiCallerFunction).Add("Name", envName);
            Amazon.CDK.Tags.Of(function).Add("Name", "DeliveryDate"); 
            Amazon.CDK.Tags.Of(apiGateway).Add("Name", "DeliveryDate");
            // Amazon.CDK.Tags.Of(cloudWatchEvent).Add("Name", envName);
        }

        private Function CreateLambdaFunction(DeliveryDateCdkStackProps props)
        {
            var fileName = new CfnParameter(
                this,
                "LamdaSyncFileName",
                new CfnParameterProps {Description = "Sync Lamda Filename", Type = "String"});

            var function = new Function(
                this,
                "DeliveryDateFunction",
                new FunctionProps
                {
                    FunctionName = $"DeliveryDateFunction",
                    Runtime = Runtime.DOTNET_CORE_3_1,
                    Code = Code.FromBucket(props.Bucket, fileName.ValueAsString),
                    MemorySize = 256,
                    Handler = "open-universe-portal-sync::OpenUniversePortalSync.Function::FunctionHandler",
                    Timeout = Duration.Minutes(15),
                    CurrentVersionOptions =
                        new VersionProps
                        {
                            Description = fileName.ValueAsString, RemovalPolicy = RemovalPolicy.RETAIN
                        },
                });
            
            return function;
        }
        
        private LambdaRestApi CreateApiGateway(DeliveryDateCdkStackProps props, IFunction backend)
        {
            var lambdaRestApi = new LambdaRestApi(this, "myapi", new LambdaRestApiProps {
                Handler = backend
            });

            return lambdaRestApi;
        }
    }
}

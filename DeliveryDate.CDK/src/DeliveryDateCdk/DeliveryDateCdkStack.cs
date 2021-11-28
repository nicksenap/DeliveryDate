using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.APIGateway;
using Construct = Constructs.Construct;


namespace DeliveryDateCdk
{

    public class DeliveryDateCdkStackProps : StackProps
    {
    }
    public class DeliveryDateCdkStack : Stack
    {
        internal DeliveryDateCdkStack(Construct scope, string id, DeliveryDateCdkStackProps props = null) : base(scope, id, props)
        {
            Create(props);
        }
        
        private void Create(DeliveryDateCdkStackProps props)
        {
            var bucket = CreateBucket(props);
            var function = CreateLambdaFunction(props, bucket);
            var apiGateway = CreateApiGateway(props, function);

            Amazon.CDK.Tags.Of(bucket).Add("Name", "DeliveryDate"); 
            Amazon.CDK.Tags.Of(function).Add("Name", "DeliveryDate"); 
            Amazon.CDK.Tags.Of(apiGateway).Add("Name", "DeliveryDate");
        }


        private IBucket CreateBucket(DeliveryDateCdkStackProps props)
        {
            var bucket = new Bucket(this, "DeliveryDateBucket", new BucketProps()
            {
                BucketName = "delivery-date-bucket",

            });
            
            return bucket;
        }
        private Function CreateLambdaFunction(DeliveryDateCdkStackProps props, IBucket bucket)
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
                    Code = Code.FromBucket(bucket, fileName.ValueAsString),
                    MemorySize = 256,
                    Handler = "open-universe-portal-sync::OpenUniversePortalSync.Function::FunctionHandler",
                    Timeout = Duration.Minutes(15),
                    CurrentVersionOptions = new VersionProps
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

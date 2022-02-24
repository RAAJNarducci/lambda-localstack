using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Nest;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Example.Lambda
{

    public class Function
    {
        private readonly IElasticClient _elasticClient;

        public Function()
        {
            _elasticClient = CreateConnection();
        }

        public async Task<string> FunctionHandler(string input, ILambdaContext context)
        {
            var isCreated = await CreateIndexAsync();
            var response = await InsertAsync(new Example
            {
                Cpf = "41251515111",
                Name = "José"
            });
            if (response)
            {
                Console.WriteLine("deu bom");
            }
            // var result = await _elasticClient.PingAsync();
            // if (response.IsValid)
            // {
            //     Console.WriteLine("Deu Ping");
            //     Console.WriteLine(result.ApiCall.HttpStatusCode);
            // }
            // else
            // {
            //     Console.WriteLine("Nao Deu Ping");
            //     Console.WriteLine($"Erro: " + result.OriginalException);
            // }

            return input.ToUpper();
        }

        private async Task<bool> CreateIndexAsync()
        {
            if (!(await _elasticClient.Indices.ExistsAsync("indice-default")).Exists)
            {
                await _elasticClient.Indices.CreateAsync("indice-default", c =>
                {
                    c.Map<Example>(p => p.AutoMap());
                    return c;
                });
            }
            return true;
        }

        public async Task<bool> InsertAsync(Example model)
        {
            var response = await _elasticClient.IndexAsync(model, descriptor => descriptor.Index("indice-default"));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return true;
        }


        private ElasticClient CreateConnection()
        {
            var settings = new ConnectionSettings(new Uri("http://es-container:9200"));
            var defaultIndex = "indice-default";

            settings = settings.DefaultIndex(defaultIndex);

            settings = settings.BasicAuthentication("ElasticUser", "ElasticPass");

            return new ElasticClient(settings);
        }
    }

    public class Example
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
    }
}

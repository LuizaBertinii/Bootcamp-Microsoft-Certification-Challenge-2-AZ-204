using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace fnPostDatabase
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("movie")]
        [CosmosDBOutput("%Database%", "%ContainerName%", Connection = "%CosmosDBConnection%", CreateIfNotExists = true)]
        public async Task<object?> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            MovieRequest movie = null;

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                movie = JsonConvert.DeserializeObject<MovieRequest>(content);
            } 
            catch (Exception ex)
            {
                return new BadRequestObjectResult("erro ao deserializar o objeto: " + ex.Message); 
            }

            return JsonConvert.DeserializeObject(movie.Id);
        }
    }
}
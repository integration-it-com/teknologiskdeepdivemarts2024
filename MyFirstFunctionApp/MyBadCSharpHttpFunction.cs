using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace deepdive
{
    public class MyBadCSharpHttpFunction
    {
        private readonly ILogger _logger;

        public MyBadCSharpHttpFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MyFirstCSharpHttpFunction>();
        }

        [Function("MyBadCSharpHttpFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request. This will not work!!!!");
            try
            {
                throw new Exception("The Function App crashed");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error was ignored");
            }
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Velkommen til Teknologisk Kursus!!!!!! I deep dive, nu med pipelines igen igen");

            return response;
        }
    }
}

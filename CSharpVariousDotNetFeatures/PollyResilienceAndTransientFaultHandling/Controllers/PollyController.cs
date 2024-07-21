using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;

namespace PollyResilienceAndTransientFaultHandling.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PollyController : ControllerBase
    {
        private readonly int _maxretries = 3;
        private readonly int _retryInterval = 2;

        [HttpGet(Name = "200")]
        public async Task<string> GetOk()
        {
            var result = new HttpResponseMessage();
            
            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = DefinePollyRetryPolicy();

            result = await retryPolicy.ExecuteAsync(CallAvailableAPI);

            LogResults(result);

            return result.ReasonPhrase.ToString();
        }

        [HttpGet(Name = "503")]
        public async Task<string> GetServiceUnavailableError()
        {
            var result = new HttpResponseMessage();

            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = DefinePollyRetryPolicy();

            result = await retryPolicy.ExecuteAsync(CallUnavailableAPI);
            
            LogResults(result);

            return result.ReasonPhrase.ToString();
        }

        private static void LogResults(HttpResponseMessage result)
        {
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Request succeeded");
            }
            else
            {
                Console.WriteLine("Request failed after retries");
            }
        }

        private AsyncRetryPolicy<HttpResponseMessage> DefinePollyRetryPolicy()
        {
            var retryPolicy = Policy
                        .Handle<HttpRequestException>()
                        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                        .WaitAndRetryAsync(_maxretries, retryAttempt => TimeSpan.FromSeconds(_retryInterval), (result, timeSpan, retryCount, context) =>
                        {
                            Console.WriteLine($"Retry {retryCount} after {timeSpan.Seconds} seconds due to {result.Result.ReasonPhrase ?? result.Result.StatusCode.ToString()}");
                        });

            return retryPolicy;
        }

        private static async Task<HttpResponseMessage> CallAvailableAPI()
        {
            var response = await new HttpClient().GetAsync("https://localhost:7118/HttpStatusCode/ok");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Request failed with status code {response.ReasonPhrase}");
            }
            return response;
        }

        private static async Task<HttpResponseMessage> CallUnavailableAPI()
        {
            var response = await new HttpClient().GetAsync("https://localhost:7118/HttpStatusCode/serviceunavailable");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Request failed with status code {response.ReasonPhrase}");
            }
            return response;
        }
    }
}

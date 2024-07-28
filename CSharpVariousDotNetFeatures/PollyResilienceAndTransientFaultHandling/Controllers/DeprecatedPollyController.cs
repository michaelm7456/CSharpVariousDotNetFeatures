using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;

namespace PollyResilienceAndTransientFaultHandling.Controllers
{
    public class DeprecatedPollyController : Controller
    {
        private readonly int _maxretries = 3;
        private readonly int _retryInterval = 2;

        [HttpGet(Name = "200 - Retry Once")]
        public async Task<string> GetOk()
        {
            var result = new HttpResponseMessage();

            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = DefinePollyRetryOncePolicy();

            result = await retryPolicy.ExecuteAsync(CallAvailableAPI);

            LogResults(result);

            return result.ReasonPhrase.ToString();
        }

        [HttpGet(Name = "503 - Fixed Time")]
        public async Task<string> GetServiceUnavailableErrorWithFixedTimeIntervalPolicy()
        {
            var result = new HttpResponseMessage();

            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = DefinePollyWaitAndRetryWithFixedIntervalsPolicy();

            result = await retryPolicy.ExecuteAsync(CallUnavailableAPI);

            LogResults(result);

            return result.ReasonPhrase.ToString();
        }

        [HttpGet(Name = "503 - Exponential BackOff")]
        public async Task<string> GetServiceUnavailableErrorWithExponentialBackOffPolicy()
        {
            var result = new HttpResponseMessage();

            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = DefinePollyWaitAndRetryWithExponentialBackOffPolicy();

            result = await retryPolicy.ExecuteAsync(CallUnavailableAPI);

            LogResults(result);

            return result.ReasonPhrase.ToString();
        }

        private AsyncRetryPolicy<HttpResponseMessage> DefinePollyRetryOncePolicy()
        {
            var retryPolicy = Policy
                        .Handle<HttpRequestException>()
                        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                        .RetryAsync();

            return retryPolicy;
        }

        private AsyncRetryPolicy<HttpResponseMessage> DefinePollyWaitAndRetryWithFixedIntervalsPolicy()
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

        private AsyncRetryPolicy<HttpResponseMessage> DefinePollyWaitAndRetryWithExponentialBackOffPolicy()
        {
            var retryPolicy = Policy
                        .Handle<HttpRequestException>()
                        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                        .WaitAndRetryAsync(
                        retryCount: _maxretries,
                        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(_retryInterval, attempt)), // Exponential backoff
                        onRetry: (response, timespan, retryCount, context) =>
                        {
                            Console.WriteLine($"Retry {retryCount} after {timespan.Seconds} seconds due to: {response.Exception?.Message ?? response.Result.ReasonPhrase}");
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
    }
}

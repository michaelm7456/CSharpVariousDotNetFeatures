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
        private readonly IHttpClientFactory _httpClientFactory;

        public PollyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "200 - Retry Once")]
        public async Task<string> GetOkUsingOldPollyRetryPolicy()
        {
            var result = new HttpResponseMessage();

            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = DefinePollyRetryOncePolicy();

            result = await retryPolicy.ExecuteAsync(CallAvailableAPI);

            LogResults(result);

            return result.ReasonPhrase.ToString();
        }

        [HttpGet(Name = "503 - Fixed Time")]
        public async Task<string> GetServiceUnavailableErrorUsingNewPollyResiliencePipelineBuilder()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = _maxretries,
                    BackoffType = DelayBackoffType.Constant,
                    Delay = TimeSpan.FromSeconds(_retryInterval),
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>().HandleResult(result => !result.IsSuccessStatusCode),
                    OnRetry = retryArguments =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Current attempt: {retryArguments.AttemptNumber}, Failure Reason: {retryArguments.Outcome.Result.ReasonPhrase}");
                        Console.ForegroundColor = ConsoleColor.White;

                        return ValueTask.CompletedTask;
                    }
                })
                .Build();

            var result = await pipeline.ExecuteAsync(async token => await httpClient.GetAsync("https://localhost:7118/HttpStatusCode/serviceunavailable"));

            return result.ReasonPhrase.ToString();
        }

        [HttpGet(Name = "503 - Exponential BackOff")]
        public async Task<string> GetServiceUnavailableErrorUsingOldPollyExponentialBackOffPolicy()
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
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"Retry {retryCount} after {timespan.Seconds} seconds due to: {response.Exception?.Message ?? response.Result.ReasonPhrase}");
                            Console.ForegroundColor = ConsoleColor.White;
                        });

            return retryPolicy;
        }

        private static async Task<HttpResponseMessage> CallAvailableAPI()
        {
            var response = await new HttpClient().GetAsync("https://localhost:7118/HttpStatusCode/ok");
            if (!response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Request failed with status code {response.ReasonPhrase}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return response;
        }

        private static async Task<HttpResponseMessage> CallUnavailableAPI()
        {
            var response = await new HttpClient().GetAsync("https://localhost:7118/HttpStatusCode/serviceunavailable");
            if (!response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Request failed with status code {response.ReasonPhrase}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return response;
        }

        private static void LogResults(HttpResponseMessage result)
        {
            if (result.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Request succeeded");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Request failed after retries");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}

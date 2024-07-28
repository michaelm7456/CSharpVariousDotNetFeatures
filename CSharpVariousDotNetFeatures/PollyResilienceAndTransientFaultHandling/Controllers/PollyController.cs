using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;
using PollyResilienceAndTransientFaultHandling.Services;

namespace PollyResilienceAndTransientFaultHandling.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PollyController : ControllerBase
    {
        private readonly int _maxretries = 3;
        private readonly int _retryInterval = 2;
        private readonly IHttpClientFactory _httpClientFactory;
        private ResiliencePipelinesService _resiliencePipelinesService = new ResiliencePipelinesService();

        public PollyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "Retry Policy only")]
        public async Task<string> RetryPolicyOnly()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var result = await _resiliencePipelinesService.RetryPipeline.ExecuteAsync(async token => await httpClient.GetAsync("https://localhost:7118/HttpStatusCode/serviceunavailable", token));

            return result.ReasonPhrase.ToString();
        }

        [HttpGet(Name = "Timeout Policy only")]
        public async Task<string> TimeoutPolicyOnly()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var result = new HttpResponseMessage();

            try
            {
                result = await _resiliencePipelinesService.TimeoutComplexPipeline.ExecuteAsync(async token => await httpClient.GetAsync("https://localhost:7118/HttpStatusCode/delay", token));
            }
            catch (TimeoutRejectedException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Outer level comment");
                Console.ForegroundColor = ConsoleColor.White;
            }

            return result.ReasonPhrase.ToString();
        }

        [HttpGet(Name = "Retry & Timeout combined - Unavailable Service")]
        public async Task<string> RetryAndTimeoutPolicyCombinedForUnavailableService()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var result = new HttpResponseMessage();

            try
            {
                result = await _resiliencePipelinesService.CombinedPipeline.ExecuteAsync(async token => await httpClient.GetAsync("https://localhost:7118/HttpStatusCode/serviceunavailable", token));
            }
            catch (TimeoutRejectedException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Outer level comment");
                Console.ForegroundColor = ConsoleColor.White;
            }

            return result.ReasonPhrase.ToString();
        }
        

        [HttpGet(Name = "Retry & Timeout combined - Service with Delayed result")]
        public async Task<string> RetryAndTimeoutPolicyCombinedForSlowRunningService()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var result = new HttpResponseMessage();

            try
            {
                result = await _resiliencePipelinesService.CombinedPipeline.ExecuteAsync(async token => await httpClient.GetAsync("https://localhost:7118/HttpStatusCode/delay", token));
            }
            catch (TimeoutRejectedException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Outer level comment");
                Console.ForegroundColor = ConsoleColor.White;
            }

            return result.ReasonPhrase.ToString();
        }
    }
}

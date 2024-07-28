using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;
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

        [HttpGet(Name = "503 - Fixed Time")]
        public async Task<string> GetServiceUnavailableErrorUsingNewPollyResiliencePipelineBuilder()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var result = await _resiliencePipelinesService.RetryPipeline.ExecuteAsync(async token => await httpClient.GetAsync("https://localhost:7118/HttpStatusCode/serviceunavailable", token));

            return result.ReasonPhrase.ToString();
        }
    }
}

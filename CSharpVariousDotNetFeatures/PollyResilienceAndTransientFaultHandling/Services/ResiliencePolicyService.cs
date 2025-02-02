using Polly;
using System.Net;

namespace PollyResilienceAndTransientFaultHandling.Services
{
    public class ResiliencePolicyService : IResiliencePolicyService
    {
        private readonly ILogger<ResiliencePolicyService> _logger;

        public ResiliencePolicyService(ILogger<ResiliencePolicyService> logger)
        {
            _logger = logger;
        }

        public IAsyncPolicy<HttpResponseMessage> GetResiliencePolicy(double sleepDurationProvider)
        {
            var retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r =>
                r.StatusCode == HttpStatusCode.RequestTimeout ||
                r.StatusCode == HttpStatusCode.TooManyRequests ||
                r.StatusCode == HttpStatusCode.InternalServerError ||
                r.StatusCode == HttpStatusCode.BadGateway ||
                r.StatusCode == HttpStatusCode.ServiceUnavailable ||
                r.StatusCode == HttpStatusCode.GatewayTimeout)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(sleepDurationProvider, retryAttempt)),
                onRetry: (response, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning("Retry {retryCount} after {timeSpan.TotalSeconds} seconds due to: {response.Result.StatusCode}", retryCount, timeSpan.TotalSeconds, response.Result.StatusCode);
                });

            var circuitBreakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r =>
                    r.StatusCode == HttpStatusCode.RequestTimeout ||
                    r.StatusCode == HttpStatusCode.TooManyRequests ||
                    r.StatusCode == HttpStatusCode.InternalServerError ||
                    r.StatusCode == HttpStatusCode.BadGateway ||
                    r.StatusCode == HttpStatusCode.ServiceUnavailable ||
                    r.StatusCode == HttpStatusCode.GatewayTimeout)
                .AdvancedCircuitBreakerAsync(
                    failureThreshold: 0.5, // 50% failure rate
                    samplingDuration: TimeSpan.FromSeconds(10), // over a 10-second period
                    minimumThroughput: 7, // at least 7 requests in the sampling duration
                    durationOfBreak: TimeSpan.FromSeconds(30), // break for 30 seconds)
                    onBreak: (delegateResult, breakDelay) =>
                    {
                        _logger.LogWarning("Circuit breaker has tripped because of exception: {delegateResult.Exception} for timespan:{breakDelay}", delegateResult.Exception, breakDelay);
                    },
                    onReset: () =>
                    {
                        _logger.LogWarning("Circuit breaker has reset");
                    },
                    onHalfOpen: () =>
                    {
                        _logger.LogWarning("Circuit breaker is in state half open");
                    });

            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }
    }
}

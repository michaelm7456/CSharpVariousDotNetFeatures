using Polly.Retry;
using Polly;
using System.Net;

namespace PollyResilienceAndTransientFaultHandling.Services
{
    public class ResiliencePipelinesService
    {
        public ResiliencePipeline<HttpResponseMessage> RetryPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
        .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
        {
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Constant,
            Delay = TimeSpan.FromSeconds(2),
            ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
        .Handle<HttpRequestException>().HandleResult(result =>
        result.StatusCode.Equals(HttpStatusCode.InternalServerError) ||
        result.StatusCode.Equals(HttpStatusCode.BadGateway) ||
        result.StatusCode.Equals(HttpStatusCode.ServiceUnavailable) ||
        result.StatusCode.Equals(HttpStatusCode.GatewayTimeout)
        ),
            OnRetry = (retryArguments) =>
            {
                if (retryArguments.AttemptNumber < 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Attept unsuccessful. Failure Reason: {retryArguments.Outcome.Result.ReasonPhrase}. Retry: {retryArguments.AttemptNumber + 1} in progress...");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                return ValueTask.CompletedTask;
            }
        })
            .Build();

        public ResiliencePipeline<HttpResponseMessage> TimeoutPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddTimeout(TimeSpan.FromSeconds(5))
            .Build();
    }
}

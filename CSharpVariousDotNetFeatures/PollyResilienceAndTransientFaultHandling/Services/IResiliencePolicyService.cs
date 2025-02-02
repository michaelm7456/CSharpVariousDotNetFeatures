using Polly;

namespace PollyResilienceAndTransientFaultHandling.Services
{
    public interface IResiliencePolicyService
    {
        public IAsyncPolicy<HttpResponseMessage> GetResiliencePolicy(double sleepDurationProvider);
    }
}

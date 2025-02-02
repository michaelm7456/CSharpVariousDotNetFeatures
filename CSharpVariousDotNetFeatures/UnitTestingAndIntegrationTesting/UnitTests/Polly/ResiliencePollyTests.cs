using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PollyResilienceAndTransientFaultHandling.Services;
using System.Net;

namespace UnitTestingAndIntegrationTesting.UnitTests.Polly
{
    public class ResiliencePollyTests
    {
        [Fact]
        public async Task PollyPolicyHandlerSendAsync_ShouldRetryUpTo3Times_WhenTransientErrors()
        {
            var services = new ServiceCollection();

            var fakeHttpDelegatingHandler = new FakeHttpDelegatingHandler(
                _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.GatewayTimeout)));

            var logger = Substitute.For<ILogger<ResiliencePolicyService>>();

            var resiliencePolicy = new ResiliencePolicyService(logger);
            var policy = resiliencePolicy.GetResiliencePolicy(TimeSpan.FromMilliseconds(1).TotalMilliseconds);

            services.AddHttpClient("my-httpclient", client =>
            {
                client.BaseAddress = new Uri("http://any.localhost");
            })
            .AddPolicyHandler(policy)
            .AddHttpMessageHandler(() => fakeHttpDelegatingHandler);

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("my-httpclient");
            var request = new HttpRequestMessage(HttpMethod.Get, "/any");

            var result = await sut.SendAsync(request);

            Assert.Equal(HttpStatusCode.GatewayTimeout, result.StatusCode);
            Assert.Equal(4, fakeHttpDelegatingHandler.Attempts);
        }

        [Fact]
        public async Task PollyPolicyHandlerSendAsync_ShouldStopRetrying_IfSuccessStatusCodeIsEncountered()
        {
            var services = new ServiceCollection();

            var fakeHttpDelegatingHandler = new FakeHttpDelegatingHandler(
                attempt =>
                {
                    return attempt switch
                    {
                        2 => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)),
                        _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.GatewayTimeout))
                    };
                });

            var logger = Substitute.For<ILogger<ResiliencePolicyService>>();

            var resiliencePolicy = new ResiliencePolicyService(logger);
            var policy = resiliencePolicy.GetResiliencePolicy(TimeSpan.FromMilliseconds(1).TotalMilliseconds);

            services.AddHttpClient("test-httpclient", client =>
            {
                client.BaseAddress = new Uri("http://dummy.localhost");
            })
            .AddPolicyHandler(policy)
            .AddHttpMessageHandler(() => fakeHttpDelegatingHandler);

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var sut = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("test-httpclient");
            var request = new HttpRequestMessage(HttpMethod.Get, "/any");

            var result = await sut.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(2, fakeHttpDelegatingHandler.Attempts);
        }
    }
}

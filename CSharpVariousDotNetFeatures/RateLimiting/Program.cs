using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using RateLimiting.Components;
using RateLimiting.Configuration.DefaultSettings;
using RateLimiting.Configuration.Keys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add rate limiting services
builder.Services.AddRateLimiter(options =>
{
    _ = options.AddConcurrencyLimiter(
        RateLimitingPolicies.GenericConcurrencyLimiter,
        _ => ConfigureRateLimiting(builder.Configuration));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Enable rate limiting within application
app.UseRateLimiter(new RateLimiterOptions
{
    GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        return
            RateLimitPartition.GetConcurrencyLimiter(
                RateLimitingPolicies.GenericConcurrencyLimiter,
                _ => ConfigureRateLimiting(builder.Configuration));
    }),
    RejectionStatusCode = 429
});

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


static ConcurrencyLimiterOptions ConfigureRateLimiting(IConfiguration configuration) => new()
{
    PermitLimit = configuration.GetSection(RateLimitingPolicies.RateLimitingPoliciesSection)
        .GetValue(RateLimitingPolicies.GenericConcurrencyPermitLimit, DefaultRateLimitingSettings.PermitLimit),

    QueueLimit = configuration.GetSection(RateLimitingPolicies.RateLimitingPoliciesSection)
        .GetValue(RateLimitingPolicies.GenericConcurrencyQueueLimit, DefaultRateLimitingSettings.QueueLimit),

    // TODO: Move this into the Configuration.
    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
};
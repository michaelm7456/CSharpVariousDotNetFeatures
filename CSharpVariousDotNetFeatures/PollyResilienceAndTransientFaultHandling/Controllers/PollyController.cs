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
    }
}

using Microsoft.AspNetCore.Mvc;

namespace JsonWebTokens.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;

        public TokenController(ILogger<TokenController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetToken")]
        public string GetToken()
        {
            var secretKey = "this is my custom Secret key for authentication"; // This should be a secret key stored securely
            var issuer = "your-app";
            var audience = "your-app-users";

            var jwtTokenGenerator = new JwtGenerator(secretKey, issuer, audience);
            var token = jwtTokenGenerator.GenerateToken("testuser");

            return token;
        }
    }
}

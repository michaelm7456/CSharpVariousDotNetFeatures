using Microsoft.AspNetCore.Mvc;

namespace SimpleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuitarController : ControllerBase
    {
        private static readonly string[] guitars =
        [
            "Stratocaster", "Jazzmaster", "Telecaster"
        ];

        private static readonly string[] colours =
        [
             "Natural wood", "Sunburst", "Cherry red", "Metallic blue"
        ];

        private readonly ILogger<GuitarController> _logger;

        public GuitarController(ILogger<GuitarController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetGuitar")]
        public IEnumerable<Guitar> Get()
        {
            return Enumerable.Range(0, 3).Select(index =>
                new Guitar
                {
                    Make = guitars[index],
                    Colour = colours[new Random().Next(1, colours.Length)]
                })
                .ToArray();
        }
    }
}

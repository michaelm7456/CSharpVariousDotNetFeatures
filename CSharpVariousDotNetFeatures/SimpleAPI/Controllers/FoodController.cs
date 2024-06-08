using Microsoft.AspNetCore.Mvc;

namespace SimpleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodController : ControllerBase
    {
        private static readonly string[] Foods =
        [
            "burger", "pizza", "kebab", "curry"
        ];

        private readonly ILogger<FoodController> _logger;

        public FoodController(ILogger<FoodController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetFood")]
        public IEnumerable<Food> Get()
        {
            return Enumerable.Range(0,4).Select(index => new Food
            {
                Name = Foods[index]
            })
            .ToArray();
        }
    }
}

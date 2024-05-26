using DependencyInjection.Model;
using DependencyInjection.Services.InfectedService;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection.Controllers
{
    public class InfectedController : ControllerBase
    {
        //Youtube link for reference.
        //https://www.youtube.com/watch?v=Pb2VZWoHSnA

        private readonly IInfectedService _infectedService;
        private readonly IInfectedService _infectedService2;

        public InfectedController(IInfectedService infectedService, IInfectedService infectedService2)
        {
            _infectedService = infectedService;
            _infectedService2 = infectedService2;
        }

        [HttpGet("/api/Infected/count")]
        public ActionResult<int> GetInfectedCount()
        {
            return _infectedService.GetInfectedCount();
        }

        [HttpGet("/api/Infected")]
        public ActionResult<List<Infected>> GetInfectedList()
        {
            return _infectedService.GetInfectedList();
        }

        [HttpPost("/api/Infected")]
        public ActionResult<List<Infected>> IncreaseInfected()
        {
            _infectedService.IncreaseInfected();
            return _infectedService2.GetInfectedList();
        }
    }
}

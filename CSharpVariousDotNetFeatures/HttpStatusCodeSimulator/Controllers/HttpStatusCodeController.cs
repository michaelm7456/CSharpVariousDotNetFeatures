using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HttpStatusCodeSimulator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpStatusCodeController : ControllerBase
    {
        // Returns 200 OK
        [HttpGet("ok")]
        public async Task<string> GetOk()
        {
            await Task.Delay(100); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.OK;
            return "This is a 200 OK response";
        }

        // Returns 200 OK, after a 10 second delay
        [HttpGet("delay")]
        public async Task<string> GetDelayedOk()
        {
            await Task.Delay(10000); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.OK;
            return "This is a 200 OK response";
        }

        // Returns 201 Created
        [HttpPost("created")]
        public async Task<string> PostCreated()
        {
            await Task.Delay(100); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.Created;
            return "This is a 201 Created response";
        }

        // Returns 400 Bad Request
        [HttpGet("badrequest")]
        public async Task<string> GetBadRequest()
        {
            await Task.Delay(100); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return "This is a 400 Bad Request response";
        }

        // Returns 401 Unauthorized
        [HttpGet("unauthorized")]
        public async Task<string> GetUnauthorized()
        {
            await Task.Delay(100); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "This is a 401 Unauthorized response";
        }

        // Returns 403 Forbidden
        [HttpGet("forbidden")]
        public async Task<string> GetForbidden()
        {
            await Task.Delay(100); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return "This is a 403 Forbidden response";
        }

        // Returns 404 Not Found
        [HttpGet("notfound")]
        public async Task<string> GetNotFound()
        {
            await Task.Delay(100); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return "This is a 404 Not Found response";
        }

        // Returns 500 Internal Server Error
        [HttpGet("servererror")]
        public async Task<string> GetServerError()
        {
            await Task.Delay(100); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return "This is a 500 Internal Server Error response";
        }

        // Returns 503 Server Unavailable Error
        [HttpGet("serviceunavailable")]
        public async Task<string> GetServiceUnavailableError()
        {
            await Task.Delay(100); // Simulating async work
            Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            return "This is a 503 Service Unavailable Error response";
        }
    }
}

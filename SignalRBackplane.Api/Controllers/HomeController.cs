using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRBackplane.Api.Hubs;
using System.Threading.Tasks;

namespace SignalRBackplane.Api.Controllers
{
    [ApiController, Route("")]
    public class HomeController : ControllerBase
    {
        IHttpContextAccessor HttpContextAccessor { get; }

        IHubContext<HelloHub> Hub { get; }

        public HomeController(IHttpContextAccessor httpContextAccessor
            , IHubContext<HelloHub> hub)
        {
            HttpContextAccessor = httpContextAccessor;
            Hub = hub;
        }

        [HttpGet, Route("")]
        public IActionResult Get() => Ok(new { name = "signalr backplane 1.0" });

        [HttpPost, Route("send")]
        public async Task<IActionResult> SendAsync([FromBody] HelloRequestMessage requestMessage)
        {
            var obj = new
            {
                IP = HttpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString(),
                requestMessage.ConnectionId
            };

            await Hub.Clients
                .Client(requestMessage.ConnectionId)
                .SendAsync("OnMessageReceived", obj);

            return Ok(obj);
        }

        public class HelloRequestMessage
        {
            public string ConnectionId { get; set; }
        }
    }
}
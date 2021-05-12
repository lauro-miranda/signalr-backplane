using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRBackplane.Api.Hubs
{
    public class HelloHub : Hub
    {
        IHttpContextAccessor HttpContextAccessor { get; }

        public HelloHub(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("OnConnected", new
            {
                LocalIpAddress = HttpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString(),
                Context.ConnectionId
            });
        }
    }
}
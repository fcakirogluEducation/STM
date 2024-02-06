using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Events.Events;

namespace STM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(CancellationToken token)
        {
            // kullanıcı kaydettim.

            var userCreatedEvent = new UserCreatedEvent
            {
                UserName = "ahmet",
                Email = "ahmet@outlook.com"
            };

            //var cancelToken = new CancellationTokenSource(10000);

            await publishEndpoint.Publish(userCreatedEvent, context => { context.SetAwaitAck(true); });


            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser()
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:stm2.updated.user.queue"));
            await sendEndpoint.Send(new UserUpdatedEvent() { UserName = "mehmet" });

            return Ok();
        }
    }
}
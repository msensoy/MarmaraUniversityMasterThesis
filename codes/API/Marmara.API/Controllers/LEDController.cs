using Marmara.API.Authentication;
using Marmara.API.Hubs;
using Marmara.API.Things;
using Marmara.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.API.Controllers
{
    [Authorize(Roles = Const.RoleAdmin)]
    [Route("things/[controller]")]
    [ApiController]
    public class LEDController : ThingBaseController
    {
        private readonly IHubContext<LEDHub> _hubContext;

        public LEDController(IHubContext<LEDHub> hubContext)
        {
            _hubContext = hubContext;
            thing = new LED();
        }

        [HttpPost("actions/set")]
        public IActionResult Post(LEDStatus status)
        {
            _hubContext.Clients.All.SendAsync("ReceiveData", status.data);
            ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.LED, status.data.ToString());

            return Ok($"Run. LED status updated to {status.data}");
        }

        [HttpGet("properties/status")]
        public IActionResult ReadStatus()
        {
            return Ok(ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.LED));
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var data = ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.LED);
            await _hubContext.Clients.All.SendAsync("ReceiveData", data);
            return Ok();
        }
    }

    public class LEDStatus
    {
        public int data { get; set; }
    }
}

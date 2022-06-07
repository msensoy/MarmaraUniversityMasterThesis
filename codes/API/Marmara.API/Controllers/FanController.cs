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
    public class FanController : ThingBaseController
    {
        private readonly IHubContext<FanHub> _hubContext;
    
        public FanController(IHubContext<FanHub> hubContext)
        {
            _hubContext = hubContext;
            thing = new Fan();
        }

        [HttpPost("actions/onoff")]
        public IActionResult Post(FanStatus status)
        {
            _hubContext.Clients.All.SendAsync("ReceiveData", status.data);
            var fanStatus = status.data ? Const.ON : Const.OFF;
            ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.FAN, fanStatus);
            return Ok($"Run. Fan status updated to {fanStatus}");
        }

        [HttpGet("properties/status")]
        public IActionResult Status()
        {
            return Ok(ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.FAN));
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var data = ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.FAN);
            await _hubContext.Clients.All.SendAsync("ReceiveData", data);
            return Ok();
        }
    }


    public class FanStatus
    {
        public bool data { get; set; }
    }
}

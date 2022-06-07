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
    public class CFLController : ThingBaseController
    {
        private readonly IHubContext<CFLHub> _hubContext;

        public CFLController(IHubContext<CFLHub> hubContext)
        {
            _hubContext = hubContext;
            thing = new CFL();
        }

        [HttpPost("actions/onoff")]
        public IActionResult Post(CFLStatus status)
        {
            _hubContext.Clients.All.SendAsync("ReceiveData", status.data);
            var cflStatus = status.data ? Const.ON : Const.OFF;
            ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.CFL, cflStatus);
            return Ok($"Run. CFL status updated to {cflStatus}");
        }

        [HttpGet("properties/status")]
        public IActionResult ReadStatus()
        {
            return Ok(ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.CFL));
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var data = ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.CFL);
            await _hubContext.Clients.All.SendAsync("ReceiveData", data);
            return Ok();
        }
    }

    public class CFLStatus
    {
        public bool data { get; set; }
    }
}

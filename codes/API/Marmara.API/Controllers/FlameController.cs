using Marmara.API.Hubs;
using Marmara.API.Things;
using Marmara.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.API.Controllers
{
    [Authorize]
    [Route("things/[controller]")]
    [ApiController]
    public class FlameController : ThingBaseController
    {
        private readonly IHubContext<FlameHub> _hubContext;
     
        public FlameController(IHubContext<FlameHub> hubContext)
        {
            _hubContext = hubContext;
            thing = new FlameSensor();
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var data = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.FLAME);
            await _hubContext.Clients.All.SendAsync("ReceiveData", new { data });
            return Ok();
        }
    }
}

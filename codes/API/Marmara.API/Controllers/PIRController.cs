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
    [Authorize]
    [Route("things/[controller]")]
    [ApiController]
    public class PIRController : ThingBaseController
    {
        private readonly IHubContext<PIRHub> _hubContext;

        public PIRController(IHubContext<PIRHub> hubContext)
        {
            _hubContext = hubContext;
            thing = new PIRSensor();
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var data = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.PIR);
            await _hubContext.Clients.All.SendAsync("ReceiveData", new { data });
            return Ok();
        }
  
    }
}

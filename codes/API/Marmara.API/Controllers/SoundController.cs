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
    public class SoundController : ThingBaseController
    {
        private readonly IHubContext<SoundHub> _hubContext;

        public SoundController(IHubContext<SoundHub> hubContext)
        {
            _hubContext = hubContext;
            thing = new SoundSensor();
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var data = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.SOUND);
            await _hubContext.Clients.All.SendAsync("ReceiveData", new { data });
            return Ok();
        }

    }
}

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
    public class AlarmController : ThingBaseController
    {
        private readonly IHubContext<AlarmHub> _hubContext;

        public AlarmController(IHubContext<AlarmHub> hubContext) 
        {
            _hubContext = hubContext;
            thing = new Alarm();
        }


        [HttpPost("actions/onoff")]
        public IActionResult Post(AlarmStatus status)
        {
            _hubContext.Clients.All.SendAsync("ReceiveData", status.data);
            var alarmStatus = status.data ? Const.ON : Const.OFF;
            ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.ALARM, alarmStatus);
            return Ok($"Run. Alarm status updated to {alarmStatus}");
        }

        [HttpGet("properties/status")]
        public IActionResult ReadStatus()
        {
            return Ok(ReadAndWriteFileHelper.ReadFileData(Const.DataActuator,Const.ALARM));
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            //var alarmStatus = Helper.ReadStatus(Const.ALARM);
            //var alarmSensorStatus = Helper.ReadStatus(Const.ALARMSENSOR);
            await _hubContext.Clients.All.SendAsync("ReceiveData", true);
            return Ok();
        }
    }
    public class AlarmStatus
    {
        public bool data { get; set; }
    }
}

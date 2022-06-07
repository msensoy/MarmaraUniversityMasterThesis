using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Marmara.API.Hubs;
using Marmara.Common;
using Marmara.API.Things;
using Marmara.Data;
using Newtonsoft.Json;
using System.Linq;
using System;
using static Marmara.Common.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Marmara.API.Controllers
{
    [Authorize]
    [Route("things/[controller]")]
    [ApiController]
    public class MQ135Controller : ThingBaseController
    {
        private readonly IHubContext<MQ135Hub> _hubContext;

        public MQ135Controller(IHubContext<MQ135Hub> hubContext)
        {
            _hubContext = hubContext;
            thing = new MQ135Sensor();
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var co2 = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ135, Const.CO2);
            await _hubContext.Clients.All.SendAsync("ReceiveData", new { co2 });
            return Ok();
        }

        [HttpGet]
        [Route("GetDataForWeek")]
        public string GetDataForWeek()
        {
            using var dbContext = new MarmaraDbContext();
            var data = dbContext.MQ135Datas.Where(x => x.CreatedDate > DateTime.Now.AddDays(-30)).OrderBy(x => x.CreatedDate).ToList();
            var model = JsonConvert.SerializeObject(data);
            return model;
        }
    }
}

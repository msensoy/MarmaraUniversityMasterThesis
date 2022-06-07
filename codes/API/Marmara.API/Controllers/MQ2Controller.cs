using Marmara.API.Hubs;
using Marmara.API.Things;
using Marmara.Common;
using Marmara.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.API.Controllers
{
    [Authorize]
    [Route("things/[controller]")]
    [ApiController]
    public class MQ2Controller : ThingBaseController
    {
        private readonly IHubContext<MQ2Hub> _hubContext;

        public MQ2Controller(IHubContext<MQ2Hub> hubContext)
        {
            _hubContext = hubContext;
            thing = new MQ2Sensor();
        }


        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var smoke = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.SMOKE);
            var co = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.CO);
            var lpg = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.LPG);
            await _hubContext.Clients.All.SendAsync("ReceiveData", new { smoke, co, lpg });
            return Ok();
        }

        [HttpGet]
        [Route("GetDataForWeek")]
        public string GetDataForWeek()
        {
            using var dbContext = new MarmaraDbContext();
            var data = dbContext.MQ2Datas.Where(x => x.CreatedDate > DateTime.Now.AddDays(-30)).OrderBy(x => x.CreatedDate).ToList();
            var model = JsonConvert.SerializeObject(data);
            return model;
        }
    }
}
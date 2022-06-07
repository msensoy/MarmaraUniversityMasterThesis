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
    public class DHT11Controller : ThingBaseController
    {
        private readonly IHubContext<DHT11Hub> _hubContext;

        public DHT11Controller(IHubContext<DHT11Hub> hubContext)
        {
            _hubContext = hubContext;
            thing = new DHT11Sensor();
        }

        [HttpGet("hub")]
        public async Task<IActionResult> Hub()
        {
            var temperature = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.TEMPERATURE);
            var humidity = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.HUMIDITY);
            await _hubContext.Clients.All.SendAsync("ReceiveData", new { temperature, humidity });
            return Ok();
        }

        [HttpGet]
        [Route("GetDataForWeek")]
        public string GetDataForWeek()
        {
            using var dbContext = new MarmaraDbContext();
            var data = dbContext.DHT11Datas.Where(x => x.CreatedDate > DateTime.Now.AddDays(-30)).OrderBy(x=>x.CreatedDate).ToList();
            var model = JsonConvert.SerializeObject(data);
            return model;
        }
    }
}

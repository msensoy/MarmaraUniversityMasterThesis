using Marmara.Common;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.API.Hubs
{
    public class DHT11Hub : Hub
    {
        public async Task ReceiveDataFromHub()
        {
            var data = ReadData();
            await Clients.All.SendAsync("ReceiveData", data);
        }

        private object ReadData()
        {
            var temperature = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.TEMPERATURE);
            var humidity = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.HUMIDITY);
            return new { temperature, humidity };
        }
    }
}

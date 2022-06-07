using Marmara.Common;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.API.Hubs
{
    public class MQ135Hub : Hub
    {
        public async Task ReceiveDataFromHub()
        {
            var data = ReadData();
            await Clients.All.SendAsync("ReceiveData", data);
        }

        private object ReadData()
        {
            var co2 = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ135, Const.CO2);
            return co2;
        }
    }
}

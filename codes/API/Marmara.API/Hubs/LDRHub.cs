using Marmara.Common;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.API.Hubs
{
    public class LDRHub : Hub
    {
        public async Task ReceiveDataFromHub()
        {
            var data = ReadData();
            await Clients.All.SendAsync("ReceiveData", data);
        }

        private object ReadData()
        {
            var data = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.LDR);
            return new { data };
        }
    }
}

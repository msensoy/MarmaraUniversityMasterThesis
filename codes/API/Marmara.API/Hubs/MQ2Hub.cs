using Marmara.Common;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.API.Hubs
{
    public class MQ2Hub : Hub
    {
        public async Task ReceiveDataFromHub()
        {
            var data = ReadData();
            await Clients.All.SendAsync("ReceiveData", data);
        }

        private object ReadData()
        {
            var smoke = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.SMOKE);
            var co = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.CO);
            var lpg = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.LPG);
            return new { smoke, co, lpg };
        }

        //public async Task ReadMQ2FromHub()
        //{
        //    var data = ReadValue();
        //    await Clients.All.SendAsync("ReceiveData", data);
        //}

    }
}

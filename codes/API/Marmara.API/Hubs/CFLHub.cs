using Marmara.Common;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.API.Hubs
{
    public class CFLHub : Hub
    {
        public async Task ReceiveDataFromHub()
        {
            var data = ReadData();
            await Clients.All.SendAsync("ReceiveData", data);
        }

        private object ReadData()
        {
            var data = ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.CFL);
            return data == Const.ON ? true: false ;
        }
    }
}

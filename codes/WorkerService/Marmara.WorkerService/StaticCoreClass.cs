using Marmara.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marmara.WorkerService
{
    public static class StaticCoreClass
    {
        //Raspberry Pi da Datayıp text den okuyup DB ye kaydederken , dosya açma işlemi değişiklik olarka algılanıyordu ve sürekli web servise istek atılıyordu, o yüzden data okuma işlemi yapılan yerlerde ChangedCount sıfırlanacak
        public static int ChangedCount { get; set; }

        public static int ChangedCountLimit //Raspberry de 3 kez algıladığı için buraya bu tanım yapıldı
        {
            get
            {
                var limit = 2;

#if DEBUG
                limit = 0;
#endif

                return limit;
            }
        }

        public static string[] HubSensorDataFile = { Const.ALARMSENSOR, Const.MQ2, Const.DHT11 };
        public static string[] HubActuatorDataFile = { Const.LED, Const.CFL, Const.FAN };
        public static int ChangedCountMQ2 { get; set; } 
    }
}

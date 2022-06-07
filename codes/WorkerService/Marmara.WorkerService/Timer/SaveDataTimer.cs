using Marmara.Common;
using Marmara.Data;
using Marmara.Data.Entity;
using System;
using System.Linq;
using System.Timers;
using static Marmara.Common.Helper;

namespace Marmara.WorkerService
{
    public class SaveDataTimer
    {
        private readonly Timer _timer;
        public SaveDataTimer(int frequencyMilliseconds)
        {
            _timer = new Timer(frequencyMilliseconds);
            _timer.Elapsed += TimerElapsed;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }


        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            try
            {
                SaveSensorData();
                //#if DEBUG

                //#else
                //               
                //                RunSchTask();
                //#endif
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            _timer.Start();
        }

        private void SaveSensorData()
        {
            using var dbContext = new MarmaraDbContext();

            //DHT11
            var data_DHT11 = new DHT11Data
            {
                Temperature = Convert.ToDouble(ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.TEMPERATURE)),
                Huminity = Convert.ToDouble(ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.HUMIDITY)),
                CreatedDate = DateTime.Now
            };
            dbContext.DHT11Datas.Add(data_DHT11);

            //MQ2
            var data_MQ2 = new MQ2Data();
            StaticCoreClass.ChangedCount = 0;
            data_MQ2.Co = Convert.ToInt32(Helper.ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.CO));
            StaticCoreClass.ChangedCount = 0;
            data_MQ2.Lpg = Convert.ToInt32(Helper.ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.LPG));
            StaticCoreClass.ChangedCount = 0;
            data_MQ2.Smoke = Convert.ToInt32(Helper.ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.SMOKE));
            dbContext.MQ2Datas.Add(data_MQ2);

            //MQ135
            var data_MQ135 = new MQ135Data();
            data_MQ135.Co2 = Convert.ToInt32(Helper.ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ135, Const.CO2));
            dbContext.MQ135Datas.Add(data_MQ135);


            var cnt = dbContext.SaveChanges();
            Console.WriteLine(cnt);

        }

    }
}

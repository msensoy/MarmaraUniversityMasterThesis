using Marmara.Common;
using Marmara.Data;
using Marmara.Data.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static Marmara.Common.Helper;

namespace Marmara.WorkerService
{
    public class RuleTimer
    {
        private readonly Timer _timer;
        public RuleTimer(int frequencyMilliseconds)
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
                RuleTask();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            _timer.Start();
        }


        private void RuleTask()
        {
            using var dbContext = new MarmaraDbContext();
            var activeRules = dbContext.Rules.Where(x => x.IsActive).ToList();

            for (int i = 0; i < activeRules.Count; i++)
            {
              
                if (activeRules[i].Name == Const.TEMPERATURE)
                {
                    Console.WriteLine($"ActiveRule is Temperature Value: {activeRules[i].Value}");
                    var temperature = Convert.ToDouble(ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.TEMPERATURE));
                    if (temperature > activeRules[i].Value)
                    {
                        Console.WriteLine("Fan turned on...");
                        ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.FAN, Const.ON);
                    }
                }
                else if (activeRules[i].Name == Const.LUX)
                {
                    Console.WriteLine("ActiveRule is LDR");

                    var ldrValue = Convert.ToDouble(ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.LDR));
                    var ledDimValue = Convert.ToInt32(ReadAndWriteFileHelper.ReadFileData(Const.DataActuator, Const.LED));

                    if (activeRules[i].IsGreaterThen)
                    {
                        if (ldrValue < activeRules[i].Value && ledDimValue < 100 )
                        {
                            Console.WriteLine("Dim being increased...");
                            ledDimValue++;
                            ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.LED, ledDimValue.ToString());
                            var url = ReadAndWriteFileHelper.ReadFileHubUrl(Const.LED);
                            Task.Run(() => RequestHelper.CallHubService(url));
                        }
                    }
                    else
                    {
                        if (ldrValue > activeRules[i].Value && ledDimValue > 0)
                        {
                            Console.WriteLine("Dim being reduced...");
                            ledDimValue--;
                            ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.LED, ledDimValue.ToString());
                            var url = ReadAndWriteFileHelper.ReadFileHubUrl(Const.LED);
                            Task.Run(() => RequestHelper.CallHubService(url));
                        }
                    }
                }
            }

        }
    }
}

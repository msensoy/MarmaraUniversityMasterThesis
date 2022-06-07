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
    public class ScheduleTimer
    {
        private readonly Timer _timer;
        public ScheduleTimer(int frequencyMilliseconds)
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
                SchTask();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            _timer.Start();
        }

        private void SchTask()
        {
            using var dbContext = new MarmaraDbContext();
            var willWorks = dbContext.SchTasks.Where(x => x.TaskStatus == TaskStatusEnum.WillWork).ToList();
  
            foreach (var task in willWorks)
            {
                Console.WriteLine($"Will Work: {task.ObjectName}");
                Console.WriteLine($"Rpi Local Time: {DateTime.Now.AddHours(2)}");
                int result = DateTime.Compare(task.ScheduleTime, DateTime.Now.AddHours(2));  //Rpi 2 saat geride

#if DEBUG
                result = DateTime.Compare(task.ScheduleTime, DateTime.Now);
#endif

                if (result > 0) continue;
                Console.WriteLine("Write...");
                if (task.ObjectName == Const.CFL)
                {
                    ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.CFL, task.ObjectStatus);
                    var url = ReadAndWriteFileHelper.ReadFileHubUrl(Const.CFL);
                    Task.Run(() => RequestHelper.CallHubService(url));
                }
                else if (task.ObjectName == Const.FAN)
                {
                    ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.FAN, task.ObjectStatus);
                    var url = ReadAndWriteFileHelper.ReadFileHubUrl(Const.FAN);
                    Task.Run(() => RequestHelper.CallHubService(url));
                }

                task.TaskStatus = TaskStatusEnum.Worked;
            }

            dbContext.UpdateRange(willWorks);
            dbContext.SaveChanges();
        }


    }
}

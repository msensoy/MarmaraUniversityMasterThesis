using Marmara.Common;
using Marmara.Data.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Marmara.Common.Helper;

namespace Marmara.WorkerService
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Run program");

            using var watcher = new FileSystemWatcher(Const.TxtDataFolderPath(Const.DataSensor))
            {
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
            };

            watcher.Changed += OnChanged;
            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Watched texts under {Const.TxtDataFolderPath(Const.DataSensor)}");

            int frequencyMillisecondsForSaveData = (int)(Convert.ToDouble(ReadAndWriteFileHelper.ReadFile(Const.DataMeta, Const.SAVEDATAFREQUENCY)) * 60 * 1000);
            var frequencyMillisecondsForRule = (int)(Convert.ToDouble(ReadAndWriteFileHelper.ReadFile(Const.DataMeta, Const.RULEFREQUENCY)) * 60 * 1000);
            int frequencyMillisecondsForSchedule = (int)(Convert.ToDouble(ReadAndWriteFileHelper.ReadFile(Const.DataMeta, Const.SCHEDULEFREQUENCY)) * 60 * 1000);

#if DEBUG
            frequencyMillisecondsForRule = 3000;
            frequencyMillisecondsForSchedule = 3000;
#else
                           
          frequencyMillisecondsForRule = frequencyMillisecondsForRule;
          frequencyMillisecondsForSchedule = frequencyMillisecondsForSchedule;
#endif
        
            var timerSaveData = new SaveDataTimer(frequencyMillisecondsForSaveData);
            var timerRule = new RuleTimer(frequencyMillisecondsForRule);
            var timerSchedule = new ScheduleTimer(frequencyMillisecondsForSchedule);


            // timerSaveData.Start();
            timerRule.Start();
            timerSchedule.Start();

            Console.WriteLine($"frequencyMillisecondsForSaveData: {frequencyMillisecondsForSaveData}");
            Console.WriteLine($"frequencyMillisecondsForRule: {frequencyMillisecondsForRule}");
            Console.WriteLine($"frequencyMillisecondsForSchedule: {frequencyMillisecondsForSchedule}");

            Console.ReadLine();
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.ChangeType != WatcherChangeTypes.Changed) return;
                StaticCoreClass.ChangedCount++;

                if (StaticCoreClass.ChangedCount > StaticCoreClass.ChangedCountLimit)
                {

                    StaticCoreClass.ChangedCount = 0;
                    var thingName = Path.GetFileNameWithoutExtension(e.Name).ToUpper();
                    if (thingName == Const.ALARMSENSOR)
                    {
                        RunActivityAlarm();
                    }
                    else
                    {
                        RunActivity(thingName);
                    }
                }
            }
            catch (Exception ex)
            {
                StaticCoreClass.ChangedCount = 0;
                Console.WriteLine(ex.Message + ex.Source + ex.StackTrace + ex.InnerException);
                throw;
            }
        }

        private static void RunActivityAlarm()
        {
            var oldValue = ReadAndWriteFileHelper.ReadFileData(Const.DataSensorOld, Const.ALARMSENSOR);
            var newValue = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.ALARMSENSOR);
            if (newValue == Const.ON)
            {
                var url = ReadAndWriteFileHelper.ReadFileHubUrl(Const.ALARMSENSOR);
                Console.WriteLine($"AlarmSensor for hub url: {url}");
                Task.Run(() => RequestHelper.CallHubService(url));

                var lastMailSendTime = ReadAndWriteFileHelper.GetLastSendMailTime();
                var mailFrequency = Convert.ToInt32(ReadAndWriteFileHelper.ReadFile(Const.DataMeta, Const.MAILFREQUENCY));

                if (DateTime.Compare(lastMailSendTime.AddMinutes(mailFrequency), DateTime.Now) < 0)
                {
                    Console.WriteLine("Mail will send");

                    var controlUserRepository = new ControlUserRepository();
                    var emails = controlUserRepository.GetUserListAsync().GetAwaiter().GetResult().Select(x => x.Email).ToList();
                  
                    MailHelper.Send(emails);

                    Console.WriteLine("Mail has been sent");
                    ReadAndWriteFileHelper.WriteFile(Const.DataMeta, Const.MAILSENDTIME, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                }
                ReadAndWriteFileHelper.WriteFileData(Const.DataSensorOld, Const.ALARMSENSOR, newValue);
            }

        }

        private static void RunActivity(string thing)
        {
            if (thing == Const.MQ2 && StaticCoreClass.ChangedCountMQ2 < 5)
            {
                StaticCoreClass.ChangedCountMQ2++;
                return;
            }
            else if (thing == Const.MQ2)
            {
                StaticCoreClass.ChangedCountMQ2 = 0;
            }
            var oldValue = ReadAndWriteFileHelper.ReadFile(Const.DataSensorOld, thing);
            var newValue = ReadAndWriteFileHelper.ReadFile(Const.DataSensor, thing);

            if (oldValue != newValue)
            {
                Console.WriteLine($"{thing} file was updated.");
                var url = ReadAndWriteFileHelper.ReadFileHubUrl(thing);
                Console.WriteLine($"{thing} for hub url: {url}");
                Task.Run(() => RequestHelper.CallHubService(url));

                ReadAndWriteFileHelper.WriteFile(Const.DataSensorOld, thing, newValue);
            }

        }
        private static void DoActuator(string thing)
        {
            Console.WriteLine($"{thing} file that is not in the watch list has changed.");
        }
    }
}
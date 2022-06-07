using Marmara.Common;
using Marmara.Data;
using Marmara.Data.Entity;
using Marmara.WorkerService;
using NUnit.Framework;
using System;
using System.Linq;
using static Marmara.Common.Helper;


namespace Marmara.Test
{
    public class ElapsedTest
    {
        System.Diagnostics.Stopwatch watch;


        [Test]
        public void SendMail()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            
            MailHelper.Send(null);

            watch.Stop();
            var watchElapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"Execution Time: {watchElapsed} ms");
        }

        [Test]
        public void InsertDataToDb()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            using var dbContext = new MarmaraDbContext();
            var data = new DHT11Data
            {
                Temperature = RandomHelper.RandomDouble(20, 30),
                Huminity = RandomHelper.RandomDouble(20, 30),
                CreatedDate = DateTime.Now
            };
            dbContext.DHT11Datas.Add(data);
            dbContext.SaveChanges();

            watch.Stop();
            var watchElapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"Execution Time: {watchElapsed} ms");
        }

        [Test]
        public void ReadDataFromDb()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            using var dbContext = new MarmaraDbContext();

            var data = dbContext.DHT11Datas.OrderBy(x=>x.Id).LastOrDefault();

            watch.Stop();
            var watchElapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"Execution Time: {watchElapsed} ms");
        }

        [Test]
        public void WriteDataToText()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            ReadAndWriteFileHelper.WriteFileData(Const.DataActuator, Const.LED, Const.ON);

            watch.Stop();
            var watchElapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"Execution Time: {watchElapsed} ms");
        }

        [Test]
        public void ReadDataFromText()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var co = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.CO);

            watch.Stop();
            var watchElapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"Execution Time: {watchElapsed} ms");
        }

        [Test]
        public void SignalR()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();


            var url = ReadAndWriteFileHelper.ReadFileHubUrl(Const.MQ2);
            RequestHelper.CallHubService(url);

            watch.Stop();
            var watchElapsed = watch.ElapsedMilliseconds;
            Console.WriteLine($"Execution Time: {watchElapsed} ms");
        }

        [Test]
        public void UrlGet()
        {

        }

        [Test]
        public void UrlPost()
        {

        }
    }
}

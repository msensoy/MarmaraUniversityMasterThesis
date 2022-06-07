using Marmara.Common;
using Marmara.Data;
using Marmara.Data.Entity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;


namespace Marmara.Test
{
    public class DummyData
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAddDHT11Data()
        {
            var createDate = DateTime.Now.AddDays(-7);
            using var dbContext = new MarmaraDbContext();
            List<DHT11Data> datas = new List<DHT11Data>();

            while (DateTime.Compare(createDate, DateTime.Now) == -1)
            {
                var data = new DHT11Data
                {
                    Temperature = RandomHelper.RandomDouble(20, 27),
                    Huminity = RandomHelper.RandomDouble(65, 75),
                    CreatedDate = createDate,
                    UpdatedDate = DateTime.Now.AddHours(3)
                };
                datas.Add(data);
                createDate = createDate.AddHours(2);
            }

            dbContext.DHT11Datas.AddRange(datas);
            var count = dbContext.SaveChanges();
        }


        [Test]
        public void TestAddMQ2Data()
        {
            //Random _random = new Random();
            var createDate = DateTime.Now.AddDays(-7);
            using var dbContext = new MarmaraDbContext();
            List<MQ2Data> datas = new List<MQ2Data>();
      
            int t = 100;
            while (DateTime.Compare(createDate, DateTime.Now) == -1)
            {
                var mq2Values = RandomHelper.RandomMQ2(0, 0.02);
                var data = new MQ2Data
                {
                    Co = mq2Values[0],
                    Lpg = mq2Values[1],
                    Smoke = mq2Values[2],
                    CreatedDate = createDate,
                    UpdatedDate = DateTime.Now.AddHours(3)
                };

                datas.Add(data);
                createDate = createDate.AddHours(2);
                t--;
            }

            dbContext.MQ2Datas.AddRange(datas);
            var count = dbContext.SaveChanges();
        }


        [Test]
        public void TestAddMQ135Data()
        {
            var createDate = DateTime.Now.AddDays(-7);
            using var dbContext = new MarmaraDbContext();
            List<MQ135Data> datas = new List<MQ135Data>();

            int t = 100;
            while (DateTime.Compare(createDate, DateTime.Now) == -1)
            {
                var data = new MQ135Data
                {
                    Co2 = RandomHelper.RandomDouble(10, 25),
                    CreatedDate = createDate,
                    UpdatedDate = DateTime.Now.AddHours(3)
                };


                datas.Add(data);
                createDate = createDate.AddHours(2);
                t--;
            }

            dbContext.MQ135Datas.AddRange(datas);
            var count = dbContext.SaveChanges();
        }

        [Test]
        public void TestSchTask()
        {
            using var dbContext = new MarmaraDbContext();

            var data1 = new SchTask()
            {
                ScheduleTime = DateTime.Now,
                ObjectName = Const.FAN,
                ObjectStatus = Const.ON,
                TaskStatus = TaskStatusEnum.WillWork

            };
            var data2 = new SchTask()
            {
                ScheduleTime = DateTime.Now.AddDays(-1),
                ObjectName = Const.CFL,
                ObjectStatus = Const.OFF,
                TaskStatus = TaskStatusEnum.Worked

            };

            dbContext.SchTasks.Add(data1);
            dbContext.SchTasks.Add(data2);
            dbContext.SaveChanges();


            var willWork = dbContext.SchTasks.Where(x => x.TaskStatus == TaskStatusEnum.WillWork && x.ScheduleTime >= DateTime.Now).ToList();

            foreach (var task in willWork)
            {
                //Helper.WriteStatu(task.ObjectName.ToLower(), task.ObjectStatus);
                task.TaskStatus = TaskStatusEnum.Worked;
                task.UpdatedDate = DateTime.Now.AddHours(3);
            }
            dbContext.UpdateRange(willWork);
            dbContext.SaveChanges();


        }

    }
}
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Marmara.Common;
using Marmara.Common.ThingClass;
using Marmara.Data.Entity;
using Marmara.W1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.W1.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(Const.Token_Login)))
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["tokenLogin"] = HttpContext.Session.GetString(Const.Token_Login);
            ViewData["userRole"] = HttpContext.Session.GetString(Const.Token_UserRole);

            List<Thing> thingsDescription = RequestHelper.GetRequestAPIMethod(Const.API_URL);

            var propertyValues = thingsDescription.SelectMany(x => x.Properties.Select(z => z.Value.ToString()));
            ViewData["propertyValues"] = string.Join(',', propertyValues);
            ViewData["temperature"] = thingsDescription.FirstOrDefault(x=>x.Title == "DHT11 Sensor").Properties.FirstOrDefault(x=>x.Title == "Temperature").Value;
            ViewData["humidity"] = thingsDescription.FirstOrDefault(x=>x.Title == "DHT11 Sensor").Properties.FirstOrDefault(x=>x.Title == "Humidity").Value;
            return View(thingsDescription);
        }

        [ViewData]
        public List<Chart> ChartMQ2 { get; set; }    
        
        [ViewData]
        public List<Chart> ChartMQ135 { get; set; }


        [ViewData]
        public List<Chart> ChartDht11Temp { get; set; }

        [ViewData]
        public List<Chart> ChartDht11Huminity { get; set; }


        [Route("/GetChartDHT11Temp")]
        public IActionResult GetChartDHT11Temp()

        {
            ChartDht11Temp = new List<Chart>();
            ChartDht11Huminity = new List<Chart>();

            var datas = JsonConvert.DeserializeObject<List<DHT11Data>>(RequestHelper.GetRequestForData(string.Format(@"{0}things/DHT11/GetDataForWeek", Const.API_URL)));

            var colorClass = new ColorsSet();
            var colorListForCharts = colorClass.Colors;


            var groupDataList = datas.Select(x => new { sValue = x.Temperature, sTime = x.CreatedDate }).GroupBy(x => new { Hour = x.sTime.Day, Day = x.sTime.Day }).ToList();
            var valueList = groupDataList.Select(x => x.Average(z => z.sValue)).ToList();
            var stringList = (groupDataList.Select(x => (x.First().sTime.Date.Date).ToString("dd.MM.yyyy"))).ToList();

            var color = colorListForCharts[3];

            var dataset = new ChartDataSet("TEMPERATURE", color, valueList, stringList);
            var chart = GetChart(dataset);
            chart.Options.Responsive = false;
            ChartDht11Temp.Add(chart);


            return View();
        }

        [Route("/GetChartDHT11Huminity")]
        public IActionResult GetChartDHT11Huminity()

        {
            ChartDht11Huminity = new List<Chart>();
            var datas = JsonConvert.DeserializeObject<List<DHT11Data>>(RequestHelper.GetRequestForData(string.Format(@"{0}things/DHT11/GetDataForWeek", Const.API_URL)));

            var colorClass = new ColorsSet();
            var colorListForCharts = colorClass.Colors;

            var groupDataList = datas.Select(x => new { sValue = x.Huminity, sTime = x.CreatedDate }).GroupBy(x => new { Hour = x.sTime.Day, Day = x.sTime.Day }).ToList();
            var valueList = groupDataList.Select(x => x.Average(z => z.sValue)).ToList();
            var stringList = (groupDataList.Select(x => (x.First().sTime.Date.Date).ToString("dd.MM.yyyy"))).ToList();
     
            var color = colorListForCharts[4];
         
            var dataset = new ChartDataSet("HUMINITY", color, valueList, stringList);
            var chart = GetChart(dataset);
            chart.Options.Responsive = false;
            ChartDht11Huminity.Add(chart);


            return View();
        }


        [Route("/GetChartMQ2")]
        public IActionResult GetChartMQ2()

        {
            ChartMQ2 = new List<Chart>();

            var datas = JsonConvert.DeserializeObject<List<MQ2Data>>(RequestHelper.GetRequestForData(string.Format(@"{0}things/MQ2/GetDataForWeek", Const.API_URL)));

            var colorClass = new ColorsSet();
            var colorListForCharts = colorClass.Colors;

            var groupDataList_smoke = datas.Select(x => new { sValue = x.Smoke, sTime = x.CreatedDate }).GroupBy(x => new { Hour = x.sTime.Day, Day = x.sTime.Day }).ToList();
            var valueList_smoke = groupDataList_smoke.Select(x => x.Average(z => z.sValue)).ToList();
            var stringList_smoke = (groupDataList_smoke.Select(x => (x.First().sTime.Date.Date).ToString("dd.MM.yyyy"))).ToList();

            var groupDataList_co = datas.Select(x => new { sValue = x.Co, sTime = x.CreatedDate }).GroupBy(x => new { Hour = x.sTime.Day, Day = x.sTime.Day }).ToList();
            var valueList_co = groupDataList_co.Select(x => x.Average(z => z.sValue)).ToList();
            var stringList_co = (groupDataList_co.Select(x => (x.First().sTime.Date.Date).ToString("dd.MM.yyyy"))).ToList();

            var groupDataList_lpg = datas.Select(x => new { sValue = x.Lpg, sTime = x.CreatedDate }).GroupBy(x => new { Hour = x.sTime.Day, Day = x.sTime.Day }).ToList();
            var valueList_lpg = groupDataList_lpg.Select(x => x.Average(z => z.sValue)).ToList();
            var stringList_lpg = (groupDataList_lpg.Select(x => (x.First().sTime.Date.Date).ToString("dd.MM.yyyy"))).ToList();

            var color_smoke = colorListForCharts[0];
            var color_co = colorListForCharts[1];
            var color_lpg = colorListForCharts[2];
            var datasets = new List<ChartDataSet>();
            var dataset_smoke = new ChartDataSet("SMOKE", color_smoke, valueList_smoke, stringList_smoke);
            var dataset_co = new ChartDataSet("CO", color_co, valueList_co, stringList_co);
            var dataset_lpg = new ChartDataSet("LPG", color_lpg, valueList_lpg, stringList_lpg);

            datasets.Add(dataset_smoke);
            datasets.Add(dataset_co);
            datasets.Add(dataset_lpg);

            var chart = GetOneChart(datasets);
            chart.Options.Responsive = false;
            ChartMQ2.Add(chart);

            return View();
        }

        [Route("/GetChartMQ135")]
        public IActionResult GetChartMQ135()

        {
            ChartMQ135 = new List<Chart>();

            var datas = JsonConvert.DeserializeObject<List<MQ135Data>>(RequestHelper.GetRequestForData(string.Format(@"{0}things/MQ135/GetDataForWeek", Const.API_URL)));

            var colorClass = new ColorsSet();
            var colorListForCharts = colorClass.Colors;

            var groupDataList_CO2 = datas.Select(x => new { sValue = x.Co2, sTime = x.CreatedDate }).GroupBy(x => new { Hour = x.sTime.Day, Day = x.sTime.Day }).ToList();
            var valueList_CO2 = groupDataList_CO2.Select(x => x.Average(z => z.sValue)).ToList();
            var stringList_CO2 = (groupDataList_CO2.Select(x => (x.First().sTime.Date.Date).ToString("dd.MM.yyyy"))).ToList();

         

            var color_smoke = colorListForCharts[0];
            var datasets = new List<ChartDataSet>();
            var dataset_CO2 = new ChartDataSet("AIR QUALITY", color_smoke, valueList_CO2, stringList_CO2);

            datasets.Add(dataset_CO2);

            var chart = GetOneChart(datasets);
            chart.Options.Responsive = false;
            ChartMQ135.Add(chart);

            return View();
        }

        public Chart GetChart(ChartDataSet chartDataSet)
        {
            Chart chart = new Chart
            {
                Type = Enums.ChartType.Line
            };

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            data.Datasets = new List<Dataset>();

            data.Labels = chartDataSet.XValues;

            LineDataset dataset = new LineDataset()
            {
                Label = chartDataSet.Title,
                Data = chartDataSet.YValues.Select(x => (double?)Math.Round(x, 2)).ToList(),
                Fill = "false",
                LineTension = 0.1,
                BackgroundColor = chartDataSet.Color,
                BorderColor = chartDataSet.Color,
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor> { chartDataSet.Color },
                PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor> { chartDataSet.Color },
                PointHoverBorderColor = new List<ChartColor> { chartDataSet.Color },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };
            data.Datasets.Add(dataset);

            chart.Data = data;
            return chart;
        }

        public Chart GetOneChart(List<ChartDataSet> chartDataSets)
        {
            Chart chart = new Chart();

            chart.Type = Enums.ChartType.Line;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            data.Datasets = new List<Dataset>();

            for (int i = 0; i < chartDataSets.Count; i++)
            {
                data.Labels = chartDataSets[i].XValues;

                LineDataset dataset = new LineDataset()
                {
                    Label = chartDataSets[i].Title,
                    Data = chartDataSets[i].YValues.Select(x => (double?)Math.Round(x, 2)).ToList(),
                    Fill = "false",
                    LineTension = 0.1,
                    BackgroundColor = chartDataSets[i].Color,
                    BorderColor = chartDataSets[i].Color,
                    BorderCapStyle = "butt",
                    BorderDash = new List<int> { },
                    BorderDashOffset = 0.0,
                    BorderJoinStyle = "miter",
                    PointBorderColor = new List<ChartColor> { chartDataSets[i].Color },
                    PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                    PointBorderWidth = new List<int> { 1 },
                    PointHoverRadius = new List<int> { 5 },
                    PointHoverBackgroundColor = new List<ChartColor> { chartDataSets[i].Color },
                    PointHoverBorderColor = new List<ChartColor> { chartDataSets[i].Color },
                    PointHoverBorderWidth = new List<int> { 2 },
                    PointRadius = new List<int> { 1 },
                    PointHitRadius = new List<int> { 10 },
                    SpanGaps = false
                };
                data.Datasets.Add(dataset);
            }

            chart.Data = data;
            return chart;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
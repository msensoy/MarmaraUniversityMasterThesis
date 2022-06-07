using ChartJSCore.Helpers;
using System.Collections.Generic;

namespace Marmara.W1.Models
{
    public class ChartDataSet
    {
        public ChartDataSet(string title, ChartColor color, List<double> yValues, List<string> xValues)
        {
            Title = title;
            Color = color;
            YValues = yValues;
            XValues = xValues;
        }
        public string Title { get; set; }
        public ChartColor Color { get; set; }
        public List<double> YValues { get; set; }
        public List<string> XValues { get; set; }
    }

    public class ColorsSet
    {
        public List<ChartColor> Colors { get; set; }
        public ColorsSet()
        {
            Colors = new List<ChartColor>()
            {
                new ChartColor()
                {
                    Alpha = 0.4,
                    Red = 72,
                    Green = 192,
                    Blue =192
                },
                new ChartColor()
                {
                    Alpha = 0.4,
                    Red = 255,
                    Green = 0,
                    Blue =0
                }
                ,
                new ChartColor()
                {
                    Alpha = 0.4,
                    Red = 22,
                    Green = 222,
                    Blue =22
                },
                new ChartColor()
                {
                    Alpha = 0.4,
                    Red = 22,
                    Green = 22,
                    Blue =222
                },
                     new ChartColor()
                {
                    Alpha = 0.4,
                    Red = 75,
                    Green = 180,
                    Blue =60
                }
            };
        }
    }
}

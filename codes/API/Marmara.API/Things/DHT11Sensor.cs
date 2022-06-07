using Marmara.API.Concrete;
using Marmara.Common;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class DHT11Sensor : Thing
    {
        public override string Id => base.BaseId + Const.DHT11.ToLower();
        //public override string Title => "Temperature & Humidity Sensor";
        public override string Title => "DHT11 Sensor";
        public override string Type => Const.Sensor;
        public override string Description => "The DHT11 is a basic, ultra low-cost digital temperature and humidity sensor.";
        public override string System => Const.HVAC;

        public override IEnumerable<Property> Properties
        {
            get => AddProp();
            set => base.Properties = value;
        }

        private IEnumerable<Property> AddProp()
        {
            var list = new List<Property>();
            Property TemperatureProperty = new Property()
            {
                Title = "Temperature",
                Description = "Temperature Property",
                Type = "double",
                Unit = "degree celsius",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.TEMPERATURE),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/temperature") }
            };
            list.Add(TemperatureProperty);

            Property HumidityProperty = new Property()
            {
                Title = "Humidity",
                Description = "Humidity Property",
                Type = "double",
                Unit = "percent",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.DHT11, Const.HUMIDITY),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/humidity") }
            };
            list.Add(HumidityProperty);

            return list;
        }
    }
}
using Marmara.API.Concrete;
using Marmara.Common;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{

    public class MQ135Sensor : Thing, ISensor
    {
        public override string Id => base.BaseId + Const.MQ135.ToLower();
        public override string Title => "MQ135 Sensor";
        public override string Type => Const.Sensor;
        public override string Description => "MQ-135 Gas sensor can detect gases such as Ammonia (NH3), sulfur (S), Benzene (C6H6), CO2 and other harmful gases and smoke.";
        public override string System => Const.HVAC;
        public override IEnumerable<Property> Properties
        {
            get => AddProp();
            set => base.Properties = value;
        }

        private IEnumerable<Property> AddProp()
        {
            var list = new List<Property>();
            Property CO2Level = new Property()
            {
                Title = "CO2",
                Description = "CO2 Level",
                Type = "double",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ135, Const.CO2),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/co2") }
            };
            list.Add(CO2Level);

            return list;
        }

    }
}

using Marmara.API.Concrete;
using Marmara.Common;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class MQ2Sensor : Thing, ISensor
    {
        public override string Id => base.BaseId + Const.MQ2.ToLower();
        public override string Title => "MQ2 Sensor";
        public override string Type => Const.Sensor;
        public override string Description => "The MQ2 Gas Sensor can detect concentrations of LPG, Smoke, Alcohol, Propane, Hydrogen, Methane and Carbon Monoxide in the air.";
        public override string System => Const.Safety;

        public override IEnumerable<Property> Properties
        {
            get => AddProp();
            set => base.Properties = value;
        }

        private IEnumerable<Property> AddProp()
        {
            var list = new List<Property>();
            Property COLevel = new Property()
            {
                Title = "CO",
                Description = "CO Level",
                Type = "double",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.CO),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/co") }
            };
            list.Add(COLevel);
            Property SmokeLevel = new Property()
            {
                Title = "Smoke",
                Description = "A device which detects smoke as part of a fire alarm system.",
                Type = "double",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.SMOKE),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/smoke") }
            };
            list.Add(SmokeLevel);
            Property LPGLevel = new Property()
            {
                Title = "LPG",
                Description = "Capability to monitor air quality. Usually monitors the concentration of harmful gases (such as CO2, CO or VOCs) together with PM2.5, PM10 dust particulates.",
                Type = "double",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.MQ2, Const.LPG),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/lpg") }
            };
            list.Add(LPGLevel);

            return list;
        }
    }
}
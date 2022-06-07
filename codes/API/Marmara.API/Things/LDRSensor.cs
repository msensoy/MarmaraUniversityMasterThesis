using Marmara.API.Concrete;
using Marmara.Common;
using System.Collections.Generic;
using System.Linq;
using static Marmara.Common.Helper;

namespace Marmara.API.Things
{
    public class LDRSensor : Thing, ISensor
    {
        public override string Id => base.BaseId + Const.LDR.ToLower();
        public override string Title => "LDR Sensor";
        public override string Type => Const.Sensor;
        public override string Description => "LDR is a special type of resistor that works on the photoconductivity principle means that resistance changes according to the intensity of light.";
        public override string System => Const.Lighting;
       
        public override IEnumerable<Property> Properties
        {
            get => AddProp();
            set => base.Properties = value;
        }
        private IEnumerable<Property> AddProp()
        {
            var list = new List<Property>();
            Property BrightnessProperty = new Property()
            {
                Title = "Brightness",
                Description = "Light level 0-100",
                Type = "integer",
                Value = ReadAndWriteFileHelper.ReadFileData(Const.DataSensor, Const.LDR),
                Links = new List<Link>() { new Link($"{this.Links.FirstOrDefault(x => x.Rel == Const.properties).Href}/brightness") }
            };
            list.Add(BrightnessProperty);

            return list;
        }
    }
}
